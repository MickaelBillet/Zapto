using AirZapto.WebServer.Services;
using AirZapto.WebServices.Configuration;
using AirZapto.WebServices.Helpers;
using AirZapto.WebServices.Middleware;
using Framework.Data.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.IO;
using System.Linq;

namespace AirZapto.WebServer
{
    public class Startup
    {
        #region Properties
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }
        #endregion

		#region Constructor
		public Startup(IWebHostEnvironment env, IConfiguration configuration)
		{
			this.Configuration = configuration;
			this.WebHostEnvironment = env;
		}
		#endregion

        #region Methods

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
            IdentityModelEventSource.ShowPII = true;

            services.AddCors(options =>
            {
                string[]? s = this.Configuration["CorsOrigins"]?.Split(",");

                if (this.WebHostEnvironment.IsDevelopment())
                {
                    options.AddDefaultPolicy(policy =>
                    {
                        policy.AllowAnyHeader()
                                .AllowAnyMethod()
                                .WithHeaders("X-Forwarded-Path")
                                .AllowAnyOrigin();
                    });
                }
                else
                {
                    options.AddDefaultPolicy(policy =>
                    {
                        if (s != null)
                        {
                            policy.AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .WithHeaders("X-Forwarded-Path")
                                    .WithOrigins(s);
                        }
                        else
                        {
                            throw new Exception("Cors Exception");
                        }
                    });
                }
            });
           
            services.AddAuthentication()
                    .AddJwtBearer(opt =>
                    {
                        opt.RequireHttpsMetadata = false;
                        opt.Authority = this.Configuration["Keycloak:auth-server-url"];
                        opt.Audience = this.Configuration["Keycloak:audience"];
                        if (this.WebHostEnvironment.IsDevelopment())
                        {
                            opt.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = false
                            };

                            opt.Events = new JwtBearerEvents()
                            {
                                OnAuthenticationFailed = c =>
                                {
                                    c.NoResult();
                                    c.Response.StatusCode = 500;
                                    c.Response.ContentType = "text/plain";

                                    return c.Response.WriteAsync(c.Exception.ToString());
                                }
                            };
                        }
                    });
            services.AddAuthorization(policies =>
            {
                policies.AddPolicy("administrator", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("user_realm_roles", "administrator");
                });

                policies.AddPolicy("user", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("user_realm_roles", "user", "administrator");
                });
            });
            services.AddServices();
            services.AddOptions();
            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "AirZapto.WebServer", Version = "v1" });
                options.OperationFilter<AddRequiredHeaderParameter>(AirZaptoConstants.Application_Prefix);
                OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Auth",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.OpenIdConnect,
                    OpenIdConnectUrl = new Uri($"{this.Configuration["Keycloak:auth-server-url"]}/.well-known/openid-configuration"),
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, Array.Empty<string>()}
                });
            });
            services.AddHealthChecks()
                    //Memory
                    .AddCheck<MemoryHealthCheck>("Memory", HealthStatus.Degraded, new string[] { "system" })
                    //Sqlite
                    .AddSqlite($"{this.Configuration["ConnectionStrings:DefaultConnection"]}",
                                                                failureStatus: HealthStatus.Unhealthy,
                                                                tags: new string[] { "system" },
                                                                name: "Sqlite")
                    //Log error in DB
                    .AddCheck<ErrorHealthCheck>("Error System",
                                                                failureStatus: HealthStatus.Degraded,
                                                                tags: new[] { "system" })
                    //Sensors list
                    .AddCheck<SensorHealthCheck>("Sensors Status",
                                                                failureStatus: HealthStatus.Unhealthy,
                                                                tags: new[] { "system" });

            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(20);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AirZapto.WebServer");
                });
            }

            if (env.IsProduction() || env.IsStaging())
            {
                app.UseExceptionHandler("/Error");
            }

            // Patch path base with forwarded path
            app.Use(async (context, next) =>
            {
                string? forwardedPath = context.Request.Headers["X-Forwarded-Path"].FirstOrDefault();
                if (string.IsNullOrEmpty(forwardedPath) == false)
                {
                    if (forwardedPath.Equals(AirZaptoConstants.Application_Prefix))
                    {
                        Log.Information(context.Request.PathBase);
                        Log.Information(context.Request.Path);
                    }
                }

                await next();
            });

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();            

            WebSocketOptions webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(40),
            };
            app.UseWebSockets(webSocketOptions);
            app.Map("/ws", (_app) => _app.UseMiddleware<WebSocketManagerMiddleware>(app.ApplicationServices));
            app.UseMiddleware<CustomExceptionMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = (check) => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                    AllowCachingResponses = false
                });
                endpoints.MapControllers();
            });

            app.Run(async context =>
			{
				string error = "Bad Request" + " : " + context.Request.Scheme + " - " + context.Request.Method + " - " + context.Request.Path;
				context.Response.StatusCode = 400;
				await context.Response.WriteAsync(error);
                Log.Error(error);
            });

            Log.Logger = app.UseSerilog(this.Configuration);
            app.ConfigureDatabase(this.Configuration);
        }
		#endregion
	}
}

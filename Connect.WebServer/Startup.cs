using Connect.Model;
using Connect.Server.Configuration;
using Connect.Server.Helpers;
using Connect.WebApi.Middleware;
using Connect.WebServer.Services;
using Framework.Infrastructure.Services;
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
using System.Linq;

namespace Connect.WebServer
{
    public class Startup
    {
        #region Property
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        #endregion

        #region Constructor
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.Environment = env;
        }
        #endregion

        #region Method
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddCors(options =>
            {
                string[]? s = this.Configuration["CorsOrigins"]?.Split(",");

                if (this.Environment.IsDevelopment())
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
            services.Configure<MailSettings>(this.Configuration.GetSection("MailSettings"));
            services.AddAuthentication()
                    .AddJwtBearer(opt =>
                    {
                        opt.RequireHttpsMetadata = false;
                        opt.Authority = this.Configuration["Keycloak:auth-server-url"];
                        opt.Audience = this.Configuration["Keycloak:audience"];
                        if (this.Environment.IsDevelopment())
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
            services.AddServices(this.Configuration);
            services.AddOptions();
            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Connect.WebServer", Version = "v1" });
                options.OperationFilter<AddRequiredHeaderParameter>(ConnectConstants.Application_Prefix);
                var securityScheme = new OpenApiSecurityScheme
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
            services.AddSignalR();
            services.AddSingleton<HostedServiceHealthCheck>();

            (string connectionString, string serverName) = ConnectionString.GetConnectionString(this.Configuration, "ConnectionStringConnect", "ServerTypeConnect");
            services.AddHealthChecks()
                    //Memory
                    .AddCheck<MemoryHealthCheck>("Memory", HealthStatus.Degraded, new string[] { "system" })
                    //Sqlite
                    .AddSqlite(connectionString: connectionString,
                                failureStatus: HealthStatus.Unhealthy,
                                tags: new string[] { "system" },
                                name: serverName)
                    //Arduino server
                    .AddCheck<HostedServiceHealthCheck>("Server Iot Connection",
                                                                failureStatus: HealthStatus.Unhealthy,
                                                                tags: new[] { "services" })
                    //Log error in DB
                    .AddCheck<ErrorHealthCheck>("Error System",
                                                                failureStatus: HealthStatus.Degraded,
                                                                tags: new[] { "system" })
                    //SignalR
                    .AddSignalRHub(this.Configuration["UrlSignalR"] + WebConstants.SignalR_Prefix,
                                                                name: "SignalR",
                                                                failureStatus: HealthStatus.Degraded,
                                                                tags: new[] { "services" })
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
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Connect.WebServer");
                });
            }

            if (env.IsProduction() || env.IsStaging())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWebSockets();

            // Patch path base with forwarded path
            app.Use(async (context, next) =>
			{
                if (context.Request.Headers != null)
                {
                    if ((context.Request.Headers["Upgrade"] == "websocket") && (context.Request.Path == "/ws"))
                    {
                        //await WebSocketHelper.Echo(context);
                        await WebSocketHelper.Process(app.ApplicationServices, context);
                    }
                    else
                    {
                        string? forwardedPath = context.Request.Headers["X-Forwarded-Path"].FirstOrDefault();
                        if (string.IsNullOrEmpty(forwardedPath) == false)
                        {
                            if (forwardedPath.Equals(ConnectConstants.Application_Prefix))
                            {
                                Log.Information(context.Request.PathBase);
                                Log.Information(context.Request.Path);
                            }
                        }
                    }
				}

                await next();
			});			
            app.UseMiddleware<CustomExceptionMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks(@"/health", new HealthCheckOptions()
                {
                    Predicate = (check) => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,                    
                    AllowCachingResponses = false
                });
                endpoints.MapHub<ConnectHub>(WebConstants.SignalR_Prefix);
                endpoints.MapControllers();
            });
            app.Run(async context =>
            {
                string error = "Bad Request" + " : " + context.Request.Scheme + " - " + context.Request.Method + " - " + context.Request.Path;
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(error);
                Log.Error(error);
            });
        }
        #endregion
    }
}

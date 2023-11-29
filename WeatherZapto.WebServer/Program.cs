using Framework.Data.Services;
using Framework.Infrastructure.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using WeatherZapto;
using WeatherZapto.WebServer.Configuration;
using WeatherZapto.WebServer.Helpers;
using WeatherZapto.WebServer.Middleware;
using WeatherZapto.WebServer.Services;

var builder = WebApplication.CreateBuilder(args);

IdentityModelEventSource.ShowPII = true;

if (builder.Environment.IsProduction())
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(5010);
    });
}

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy<string, RateLimiterPolicy>("SlidingPolicy");
});
builder.Services.AddCors(options =>
{
    string[]? s = builder.Configuration["CorsOrigins"]?.Split(",");

    if (builder.Environment.IsDevelopment())
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
builder.Services.AddHttpClient("OpenWeather", client =>
{
    client.BaseAddress = new Uri($"{builder.Configuration["ProtocolController"]}://{builder.Configuration["OpenWeather:BackendUrl"]}/");
    client.Timeout = new TimeSpan(100000000); //10 sec
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
});
builder.Services.AddAuthentication()
                   .AddJwtBearer(opt =>
                   {
                       opt.RequireHttpsMetadata = false;
                       opt.Authority = builder.Configuration["Keycloak:auth-server-url"];
                       opt.Audience = builder.Configuration["Keycloak:audience"];
                       if (builder.Environment.IsDevelopment())
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
builder.Services.AddAuthorization(policies =>
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
builder.Services.AddMemoryCache();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddOptions();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "WeatherZapto.WebServer", Version = "v1" });
    options.OperationFilter<AddRequiredHeaderParameter>(WeatherZaptoConstants.Application_Prefix);
    OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme
    {
        Name = "Auth",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.OpenIdConnect,
        OpenIdConnectUrl = new Uri($"{builder.Configuration["Keycloak:auth-server-url"]}/.well-known/openid-configuration"),
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

builder.Services.AddHealthChecks()
        //Memory
        .AddCheck<MemoryHealthCheck>("Memory", 
                                        HealthStatus.Degraded, 
                                        new string[] { "system" })
        //PostGreSql
        .AddNpgSql(builder.Configuration["ConnectionStrings:DefaultConnection"]!, 
                            name:"PostGreSql", 
                            failureStatus:HealthStatus.Unhealthy, 
                            tags: new string[] {"system"})
        //Log error in DB
        .AddCheck<ErrorHealthCheck>("Error System",
                                        failureStatus: HealthStatus.Degraded,
                                        tags: new[] { "system" });

builder.Services.Configure<HealthCheckPublisherOptions>(options =>
{
    options.Delay = TimeSpan.FromSeconds(20);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeatherZapto.WebServer");
    });
}

// Patch path base with forwarded path
app.Use(async (context, next) =>
{
    string? forwardedPath = context.Request.Headers["X-Forwarded-Path"].FirstOrDefault();
    if (string.IsNullOrEmpty(forwardedPath) == false)
    {
        if (forwardedPath.Equals(WeatherZaptoConstants.Application_Prefix))
        {
            Log.Information(context.Request.PathBase);
            Log.Information(context.Request.Path);
        }
    }

    await next();
});

app.UseRouting();
app.UseRateLimiter();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<CustomExceptionMiddleware>();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks(@"/health", new HealthCheckOptions()
    {
        Predicate = (check) => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        AllowCachingResponses = false
    });
    endpoints.MapControllers();
});

var startupTasks = app.Services.GetServices<IStartupTask>();
foreach (var task in startupTasks)
{
    await task.Execute();
}

app.Run();




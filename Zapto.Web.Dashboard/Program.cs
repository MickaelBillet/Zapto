using AirZapto;
using Blazored.LocalStorage;
using Connect.Model;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using WeatherZapto;
using Zapto.Component.Services.Helpers;
using Zapto.Web.Dashboard;
using Zapto.Web.Dashboard.Configuration;
using Zapto.Web.Dashboard.Extensions;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("ConnectClient", client =>
{
    client.BaseAddress = new Uri($"{builder.Configuration["ProtocolController"]}://{builder.Configuration["BackEndUrl"]}:{builder.Configuration["PortHttpsConnect"]}/{builder.Configuration["PathBaseConnect"]}");
    client.Timeout = new TimeSpan(100000000); //10 sec
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
    client.DefaultRequestHeaders.Add("X-Forwarded-Path", ConnectConstants.Application_Prefix);
})
.AddHttpMessageHandler(sp =>
{
    var handler = sp.GetRequiredService<AuthorizationMessageHandler>()
    .ConfigureHandler(authorizedUrls: new[] { $"{builder.Configuration["ProtocolController"]}://{builder.Configuration["BackEndUrl"]}:{builder.Configuration["PortHttpsConnect"]}" });
    return handler;
});

builder.Services.AddHttpClient("AirZaptoClient", client =>
{
	client.BaseAddress = new Uri($"{builder.Configuration["ProtocolController"]}://{builder.Configuration["BackEndUrl"]}:{builder.Configuration["PortHttpsAirZapto"]}/{builder.Configuration["PathBaseAirZapto"]}");
	client.Timeout = new TimeSpan(100000000); //10 sec
	client.DefaultRequestHeaders.Accept.Clear();
	client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
    client.DefaultRequestHeaders.Add("X-Forwarded-Path", AirZaptoConstants.Application_Prefix);
})
.AddHttpMessageHandler(sp =>
 {
     var handler = sp.GetRequiredService<AuthorizationMessageHandler>()
     .ConfigureHandler(authorizedUrls: new[] { $"{builder.Configuration["ProtocolController"]}://{builder.Configuration["BackEndUrl"]}:{builder.Configuration["PortHttpsAirZapto"]}" });
     return handler;
 });

builder.Services.AddHttpClient("WeatherZaptoClient", client =>
{
    client.BaseAddress = new Uri($"{builder.Configuration["ProtocolController"]}://{builder.Configuration["BackEndUrl"]}:{builder.Configuration["PortHttpsWeatherZapto"]}/{builder.Configuration["PathBaseWeatherZapto"]}");
    client.Timeout = new TimeSpan(100000000); //10 sec
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
    client.DefaultRequestHeaders.Add("X-Forwarded-Path", WeatherZaptoConstants.Application_Prefix);
});

builder.Services.AddHttpClient("OpenWeather", client =>
{
	client.BaseAddress = new Uri($"{builder.Configuration["ProtocolController"]}://{builder.Configuration["OpenWeather:BackendUrl"]}/");
	client.Timeout = new TimeSpan(100000000); //10 sec
	client.DefaultRequestHeaders.Accept.Clear();
	client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
});

builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.MetadataUrl = builder.Configuration["Keycloak:MetadataUrl"];
    options.ProviderOptions.Authority = builder.Configuration["Keycloak:Authority"];
    options.ProviderOptions.ClientId = builder.Configuration["Keycloak:ClientId"];
    options.ProviderOptions.PostLogoutRedirectUri = builder.Configuration["Keycloak:PostLogoutRedirectUri"];
    options.ProviderOptions.ResponseType = "code";
    options.UserOptions.RoleClaim = "user_realm_roles";
}).AddAccountClaimsPrincipalFactory<AccountClaimsPrincipalFactoryCustom>();

builder.Services.AddAuthorizationCore(policies =>
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
builder.Services.AddOptions();
builder.Services.AddLocalization();
builder.Services.AddMudServices();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddViewModels();
builder.Services.AddBlazoredLocalStorage();

WebAssemblyHost host = builder.Build();
var logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();
logger.LogInformation("Application Started");
await host.SetDefaultCulture();
await host.RunAsync();
using AirZapto;
using Blazored.LocalStorage;
using Connect.Model;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Serilog;
using Serilog.Debugging;
using WeatherZapto;
using Zapto.Component.Services.Helpers;
using Zapto.Web.Dashboard;
using Zapto.Web.Dashboard.Configuration;
using Zapto.Web.Dashboard.Extensions;

SelfLog.Enable(m => Console.Error.WriteLine(m));

Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.WithProperty("InstanceId", Guid.NewGuid().ToString("n"))
                    .WriteTo.BrowserConsole()
                    .CreateLogger();

Log.Debug("Hello, Zapto !");


WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");


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
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 60000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

WebAssemblyHost host = builder.Build();
await host.SetDefaultCulture();
await host.RunAsync();
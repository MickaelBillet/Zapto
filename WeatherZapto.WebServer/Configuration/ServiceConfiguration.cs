using Framework.Data.Services;
using Framework.Infrastructure.Abstractions;
using Framework.Infrastructure.Services;
using WeatherZapto.Application;
using WeatherZapto.Data;
using WeatherZapto.Data.Repository;
using WeatherZapto.Data.Services;
using WeatherZapto.Infrastructure.Services;
using WeatherZapto.WebServer.Services;

namespace WeatherZapto.WebServer.Configuration
{
    public static class ServiceConfiguration
	{
		public static void AddServices(this IServiceCollection services, IConfiguration configuration)
		{
            services.AddSecretService(configuration);
            services.AddTransient<IHttpClientService, HttpClientService>((service) => new HttpClientService(service, configuration));
            services.AddTransient<IInternetService, InternetServiceWeb>();
            services.AddOpenWeatherWebServices(configuration, "OpenWeather");
            services.AddSingleton<IDatabaseService, WeatherZaptoDatabaseService>(provider => new WeatherZaptoDatabaseService(provider, WeatherZaptoConstants.ConnectionStringWeatherZaptotKey, WeatherZaptoConstants.ServerTypeWeatherZaptoKey));
            services.AddSingleton<IHostedService, HomeAirPollutionAcquisitionService>();
            services.AddSingleton<IHostedService, HomeWeatherAcquisitionService>();
            Version softwareVersion = new Version(configuration["Version"] ?? "0.0.0");
            services.AddTransient<IStartupTask, CreateDatabaseStartupTask>((provider) => new CreateDatabaseStartupTask(provider, softwareVersion.Major, softwareVersion.Minor, softwareVersion.Build));
            services.AddTransient<IStartupTask, LoggerStartupTask>();
            services.AddApplicationWeaterZaptoServices();
            services.AddSingleton<CacheSignal>();
            services.AddSupervisor();
            services.AddRepositories(WeatherZaptoConstants.ConnectionStringWeatherZaptotKey, WeatherZaptoConstants.ServerTypeWeatherZaptoKey);
            services.AddTransient<IErrorHandlerWebService, ErrorHandlerWebService>();
        }
    }
}

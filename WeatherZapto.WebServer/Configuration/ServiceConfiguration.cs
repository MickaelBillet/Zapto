using Framework.Data.Abstractions;
using Framework.Data.Services;
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
            services.AddTransient<IHttpClientService, HttpClientService>((service) => new HttpClientService(service, configuration));
            services.AddTransient<IInternetService, InternetServiceWeb>();
            services.AddOpenWeatherWebServices(configuration, "OpenWeather");
            services.AddSingleton<IDatabaseService, WeatherZaptoDatabaseService>();
            services.AddSingleton<IHostedService, HomeAirPollutionAcquisitionService>();
            services.AddSingleton<IHostedService, HomeWeatherAcquisitionService>();
            services.AddTransient<IStartupTask, DatabaseStartupTask>();
            services.AddTransient<IStartupTask, LoggerStartupTask>();
            services.AddApplicationWeaterZaptoServices();
            services.AddSingleton<CacheSignal>();
            services.AddSupervisor();
            services.AddRepositories();
            services.AddTransient<IErrorHandlerWebService, ErrorHandlerWebService>();
        }
    }
}

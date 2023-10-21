using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherZapto.Application.Infrastructure;
using WeatherZapto.Infrastructure.OpenWeatherServices;
using WeatherZapto.Infrastructure.WebServices;

namespace WeatherZapto.Infrastructure.Services
{
    public static class WeatherZaptoInfrastructureServices
	{
		public static void AddOpenWeatherWebServices(this IServiceCollection services,
															IConfiguration configuration,
															string httpClientName)
		{
			services.AddTransient<IWeatherOWService, WeatherOWService>((service) =>
			{
				return new WeatherOWService(service, configuration, httpClientName);
			});
            services.AddTransient<IAirPollutionOWService, AirPollutionOWService>((service) =>
            {
                return new AirPollutionOWService(service, configuration, httpClientName);
            });
            services.AddTransient<ILocationOWService, LocationOWService>((service) =>
            {
                return new LocationOWService(service, configuration, httpClientName);
            });
        }

        public static void AddWeatherZaptoWebServices(this IServiceCollection services,
                                                            IConfiguration configuration,               
                                                            string httpClientName)
        {
            services.AddTransient<IWeatherService, WeatherService>((service) =>
            {
                return new WeatherService(service, configuration, httpClientName);
            });
            services.AddTransient<IAirPollutionService, AirPollutionService>((service) =>
            {
                return new AirPollutionService(service, configuration, httpClientName);
            });
            services.AddTransient<ILocationService, LocationService>((service) =>
            {
                return new LocationService(service, configuration, httpClientName);
            });
            services.AddTransient<IHealthCheckWeatherZaptoService, HealthCheckWeatherZaptoService>((service) =>
            {
                return new HealthCheckWeatherZaptoService(service, configuration, httpClientName);
            });
        }
    }
}

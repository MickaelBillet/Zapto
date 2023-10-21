using Microsoft.Extensions.DependencyInjection;
using WeatherZapto.Application.Services;

namespace WeatherZapto.Application
{
	public static class WeatherZaptoAppServices
	{
		public static void AddApplicationWeaterZaptoServices(this IServiceCollection services)
		{
			services.AddTransient<IApplicationWeatherService, ApplicationWeatherService>();
			services.AddTransient<IApplicationAirPollutionService, ApplicationAirPollutionService>();
			services.AddTransient<IApplicationLocationService, ApplicationLocationService>();
			services.AddTransient<IApplicationOWService, ApplicationOWService>();
			services.AddTransient<IApplicationHealthCheckWeatherZaptoServices, ApplicationHealthChechWeatherZaptoService>();
		}		
	}
}

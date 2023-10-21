using AirZapto.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AirZapto.Application
{
    public static class AirZaptoAppService
	{
		public static void AddApplicationAirZaptoServices(this IServiceCollection services)
		{
			services.AddTransient<IApplicationSensorServices, ApplicationSensorServices>();
			services.AddTransient<IApplicationSensorDataServices, ApplicationSensorDataServices>();
            services.AddTransient<IApplicationHealthCheckAirZaptoServices, ApplicationHealthCheckAirZaptoService>();
        }   
    }
}

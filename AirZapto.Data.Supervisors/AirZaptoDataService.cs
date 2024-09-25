using AirZapto.Data.Services;
using AirZapto.Data.Supervisors;
using Microsoft.Extensions.DependencyInjection;

namespace AirZapto.Data
{
    public static class AirZaptoDataService
	{
		public static void AddSupervisors(this IServiceCollection services)
		{
			services.AddTransient<ISupervisorFactoryLogs, SupervisorFactoryLogs>();
            services.AddTransient<ISupervisorFactorySensor, SupervisorFactorySensor>();
            services.AddTransient<ISupervisorFactorySensorData, SupervisorFactorySensorData>();
            services.AddTransient<ISupervisorFactoryVersion, SupervisorFactoryVersion>();

            services.AddTransient<ISupervisorCacheSensor, SupervisorCacheSensor>();
        }
    }
}

using AirZapto.Data.Supervisors;
using Microsoft.Extensions.DependencyInjection;

namespace AirZapto.Data
{
    public static class AirZaptoDataService
	{
		public static void AddSupervisors(this IServiceCollection services)
		{
			services.AddTransient<ISupervisorLogs, SupervisorLogs>();
            services.AddTransient<ISupervisorSensor, SupervisorSensor>();
            services.AddTransient<ISupervisorSensorData, SupervisorSensorData>();
            services.AddTransient<ISupervisorVersion, SupervisorVersion>();
        }
    }
}

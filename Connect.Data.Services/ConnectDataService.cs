using Connect.Data.Supervisors;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data
{
    public static class ConnectDataService
	{
		public static void AddSupervisor(this IServiceCollection services)
		{
            services.AddScoped<ISupervisorClientApps, SupervisorClientApps>();
            services.AddScoped<ISupervisorCondition, SupervisorCondition>();
            services.AddScoped<ISupervisorConfiguration, SupervisorConfiguration>();
            services.AddScoped<ISupervisorConnectedObject, SupervisorConnectedObject>();
            services.AddScoped<ISupervisorLocation, SupervisorLocation>();
            services.AddScoped<ISupervisorLog, SupervisorLog>();
            services.AddScoped<ISupervisorNotification, SupervisorNotification>();
            services.AddScoped<ISupervisorOperatingData, SupervisorOperatingData>();
            services.AddScoped<ISupervisorOperationRange, SupervisorOperationRange>();
            services.AddScoped<ISupervisorPlug, SupervisorPlug>();
            services.AddScoped<ISupervisorProgram, SupervisorProgram>();
            services.AddScoped<ISupervisorRoom, SupervisorRoom>();
            services.AddScoped<ISupervisorSensor, SupervisorSensor>();
            services.AddScoped<ISupervisorVersion, SupervisorVersion>();
        }
    }
}

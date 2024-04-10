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
            services.AddScoped<ISupervisorServerIotStatus, SupervisorServerIotStatus>();
        }

        public static void AddCacheSupervisor(this IServiceCollection services)
        {
            services.AddScoped<ISupervisorSensor, SupervisorCacheSensor>();
            services.AddScoped<ISupervisorRoom, SupervisorCacheRoom>();
            services.AddScoped<ISupervisorProgram, SupervisorCacheProgram>();
            services.AddScoped<ISupervisorPlug, SupervisorCachePlug>();
            services.AddScoped<ISupervisorOperationRange, SupervisorCacheOperationRange>();
            services.AddScoped<ISupervisorNotification, SupervisorCacheNotification>();
            services.AddScoped<ISupervisorLocation, SupervisorCacheLocation>();
            services.AddScoped<ISupervisorConnectedObject, SupervisorCacheConnectedObject>();
            services.AddScoped<ISupervisorConfiguration, SupervisorCacheConfiguration>();
            services.AddScoped<ISupervisorCondition, SupervisorCacheCondition>();
        }
    }
}

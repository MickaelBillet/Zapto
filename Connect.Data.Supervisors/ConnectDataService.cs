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
            services.AddScoped<ISupervisorCacheSensor, SupervisorCacheSensor>();
            services.AddScoped<ISupervisorCacheRoom, SupervisorCacheRoom>();
            services.AddScoped<ISupervisorCacheProgram, SupervisorCacheProgram>();
            services.AddScoped<ISupervisorCachePlug, SupervisorCachePlug>();
            services.AddScoped<ISupervisorCacheOperationRange, SupervisorCacheOperationRange>();
            services.AddScoped<ISupervisorCacheNotification, SupervisorCacheNotification>();
            services.AddScoped<ISupervisorCacheLocation, SupervisorCacheLocation>();
            services.AddScoped<ISupervisorCacheConnectedObject, SupervisorCacheConnectedObject>();
            services.AddScoped<ISupervisorCacheConfiguration, SupervisorCacheConfiguration>();
            services.AddScoped<ISupervisorCacheCondition, SupervisorCacheCondition>();
        }
    }
}

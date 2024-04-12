using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public static class SupervisorServices
    {
        public static void AddSupervisors(this IServiceCollection services)
        {
            services.AddScoped<ISupervisorFactoryClientApp, SupervisorFactoryClientApp>();
            services.AddScoped<ISupervisorFactoryCondition, SupervisorFactoryCondition>();
            services.AddScoped<ISupervisorFactoryConfiguration, SupervisorFactoryConfiguration>();
            services.AddScoped<ISupervisorFactoryConnectedObject, SupervisorFactoryConnectedObject>();
            services.AddScoped<ISupervisorFactoryLocation, SupervisorFactoryLocation>();
            services.AddScoped<ISupervisorFactoryLog, SupervisorFactoryLog>();
            services.AddScoped<ISupervisorFactoryNotification, SupervisorFactoryNotification>();
            services.AddScoped<ISupervisorFactoryOperatingData, SupervisorFactoryOperatingData>();
            services.AddScoped<ISupervisorFactoryOperationRange, SupervisorFactoryOperationRange>();
            services.AddScoped<ISupervisorFactoryPlug, SupervisorFactoryPlug>();
            services.AddScoped<ISupervisorFactoryProgram, SupervisorFactoryProgram>();
            services.AddScoped<ISupervisorFactoryRoom, SupervisorFactoryRoom>();
            services.AddScoped<ISupervisorFactorySensor, SupervisorFactorySensor>();
            services.AddScoped<ISupervisorFactoryServerIotStatus, SupervisorFactoryServerIotStatus>();
            services.AddScoped<ISupervisorFactoryVersion, SupervisorFactoryVersion>();
        }
    }
}

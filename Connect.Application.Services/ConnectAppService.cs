using Connect.Application.Services;
using Framework.Core.Base;
using Framework.Core.InMemoryEventBus.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Application
{
    public static class ConnectAppService
	{
		public static void AddApplicationConnectServices(this IServiceCollection services)
		{
			services.AddScoped<IApplicationConditionServices, ApplicationConditionServices>();
			services.AddScoped<IApplicationConnectedObjectServices, ApplicationConnectedObjectServices>();
			services.AddScoped<IApplicationConnectLocationServices, ApplicationConnectLocationServices>();
			services.AddScoped<IApplicationPlugServices, ApplicationPlugServices>();
			services.AddScoped<IApplicationProgramServices, ApplicationProgramServices>();
			services.AddScoped<IApplicationRoomServices, ApplicationRoomServices>();
			services.AddScoped<IApplicationSensorServices, ApplicationSensorServices>();
			services.AddScoped<IApplicationNotificationServices, ApplicationNotificationServices>();
			services.AddScoped<IApplicationServerIotServices, ApplicationServerIotServices>();
			services.AddScoped<IApplicationClientAppServices, ApplicationClientAppServices>();
			services.AddScoped<IApplicationOperationDataService, ApplicationOperationDataService>();
            services.AddTransient<IApplicationHealthCheckConnectServices, ApplicationHealthConnectCheckService>();
        }

		public static void AddInMemoryEvent(this IServiceCollection services)
		{
            services.AddInMemoryEvent<MessageArduino, ApplicationPlugServices>();
            services.AddInMemoryEvent<MessageArduino, ApplicationSensorServices>();
            services.AddInMemoryEvent<MessageArduino, ApplicationServerIotServices>();
        }
    }
}

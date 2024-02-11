using Connect.Application.Infrastructure;
using Connect.Infrastructure.WebServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Infrastructure.Services
{
    public static class ConnectInfrastructureService
	{
		public static void AddConnectWebServices(this IServiceCollection services, IConfiguration configuration, string httpClientName)
		{
			services.AddTransient<IClientAppService, ClientAppService>((service) =>
			{
				return new ClientAppService(service, httpClientName);
			});
			services.AddTransient<IConditionService, ConditionService>((service) =>
			{
				return new ConditionService(service, httpClientName);
            });
			services.AddTransient<IConnectedObjectService, ConnectedObjectService>((service) =>
			{
				return new ConnectedObjectService(service, httpClientName);
            });
			services.AddTransient<ILocationService, LocationService>((service) =>
			{
				return new LocationService(service, httpClientName);
			});
			services.AddTransient<INotificationService, NotificationService>((service) =>
			{
				return new NotificationService(service	, httpClientName);
			});
			services.AddTransient<IOperationRangeService, OperationRangeService>((service) =>
			{
				return new OperationRangeService(service	, httpClientName);
			});
			services.AddTransient<IPlugService, PlugService>((service) =>
			{
				return new PlugService(service, httpClientName);
			});
			services.AddTransient<IProgramService, ProgramService>((service) =>
			{
				return new ProgramService(service, httpClientName);
			});
			services.AddTransient<IRoomService, RoomService>((service) =>
			{
				return new RoomService(service, httpClientName);
			});
			services.AddTransient<IHealthCheckConnectService, HealthCheckConnectService>((service) =>
			{
				return new HealthCheckConnectService(service, httpClientName);
			});
			services.AddTransient<ISensorService, SensorService>((service) =>
			{
				return new SensorService(service, httpClientName);
			});
			services.AddTransient<IOperatingDataService, OperatingDataService>((service) =>
			{
				return new OperatingDataService(service, httpClientName);
			});
		}
	}
}

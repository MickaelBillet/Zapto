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
			})
			.AddTransient<IConditionService, ConditionService>((service) =>
			{
				return new ConditionService(service, httpClientName);
            })
			.AddTransient<IConnectedObjectService, ConnectedObjectService>((service) =>
			{
				return new ConnectedObjectService(service, httpClientName);
            })
			.AddTransient<ILocationService, LocationService>((service) =>
			{
				return new LocationService(service, httpClientName);
			})
			.AddTransient<INotificationService, NotificationService>((service) =>
			{
				return new NotificationService(service	, httpClientName);
			})
			.AddTransient<IOperationRangeService, OperationRangeService>((service) =>
			{
				return new OperationRangeService(service	, httpClientName);
			})
			.AddTransient<IPlugService, PlugService>((service) =>
			{
				return new PlugService(service, httpClientName);
			})
			.AddTransient<IProgramService, ProgramService>((service) =>
			{
				return new ProgramService(service, httpClientName);
			})
			.AddTransient<IRoomService, RoomService>((service) =>
			{
				return new RoomService(service, httpClientName);
			})
			.AddTransient<IHealthCheckConnectService, HealthCheckConnectService>((service) =>
			{
				return new HealthCheckConnectService(service, httpClientName);
			})
			.AddTransient<ISensorService, SensorService>((service) =>
			{
				return new SensorService(service, httpClientName);
			})
			.AddTransient<IOperatingDataService, OperatingDataService>((service) =>
			{
				return new OperatingDataService(service, httpClientName);
			});
		}
	}
}

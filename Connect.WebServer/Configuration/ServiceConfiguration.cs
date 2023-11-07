using Connect.Application;
using Connect.Application.Infrastructure;
using Connect.Data;
using Connect.Data.Database;
using Connect.Data.Repository;
using Connect.WebServer.Services;
using Framework.Data.Abstractions;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Connect.Server.Configuration
{
    public static class ServiceConfiguration
	{
		public static void AddServices(this IServiceCollection services)
		{
            services.AddMailService();
            services.AddUdpService();
            services.AddSupervisor();
            services.AddRepositories();
            services.AddSingleton<IDatabaseService, ConnectDatabaseService>(); 
            services.AddTransient<ISignalRConnectService, SignalRConnectService>();
            services.AddApplicationConnectServices();
            services.AddSingleton<IHostedService, RecordingDataService>();
            services.AddSingleton<IHostedService, DailyService>();
            services.AddSingleton<IHostedService, ProcessingDataService>();
            services.AddSingleton<IHostedService, CommandStatusService>();
            services.AddSingleton<IHostedService, SensorConnectionService>();
            services.AddSingleton<IHostedService, SensorDataService>();
            services.AddSingleton<IHostedService, SensorEventService>();
            services.AddSingleton<IHostedService, ServerIotConnectionService>();
            services.AddSingleton<IFirebaseService, FirebaseService>((provider) => new FirebaseService(provider.CreateScope(), "./connect-notification-9314f-firebase-adminsdk-m70fo-6cfc9a1076.json"));
        }
    }
}

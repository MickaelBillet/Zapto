using Connect.Application;
using Connect.Application.Infrastructure;
using Connect.Data.Supervisors;
using Connect.Model;
using Connect.Data;
using Connect.WebServer.Services;
using Framework.Common.Services;
using Framework.Data.Services;
using Framework.Infrastructure.Abstractions;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Framework.Core.Base;

namespace Connect.Server.Configuration
{
    public static class ServiceConfiguration
	{
		public static void AddServices(this IServiceCollection services, IConfiguration configuration)
		{
            services.AddSecretService(configuration);
            services.AddMailService();
            services.AddRepositories(ConnectConstants.ConnectionStringConnectKey, ConnectConstants.ServerTypeConnectKey);
            services.AddApplicationConnectServices();
            services.AddCommunicationService(configuration["CommunicationType"] ?? CommunicationType.WebSocket);
            services.AddConnectServices(configuration["CommunicationType"] ?? CommunicationType.WebSocket);
            services.AddCacheServices();
            services.AddSupervisors();
            services.AddSingleton<CacheSignal>();
            services.AddSingleton<IDatabaseService, ConnectDatabaseService>(provider => new ConnectDatabaseService(provider, ConnectConstants.ConnectionStringConnectKey, ConnectConstants.ServerTypeConnectKey));
            services.AddTransient<ISignalRConnectService, SignalRConnectService>();
            services.AddTransient<ISendMailAlertService, SendMailAlertService>();
            services.AddSingleton<IHostedService, RecordingDataService>();
            services.AddSingleton<IHostedService, DailyService>();
            services.AddSingleton<IHostedService, ProcessingDataService>();
            services.AddSingleton<IHostedService, SensorConnectionService>();
            services.AddSingleton<ISerialPortService, SerialPortService>((provider) => new SerialPortService(115200, "COM6"));
            services.AddTransient<ISerialCommunicationService, SendSerialCommunicationService>();

            Version softwareVersion = new Version(configuration["Version"] ?? "0.0.0");
            services.AddTransient<IStartupTask, CreateDatabaseStartupTask>((provider) => new CreateDatabaseStartupTask(provider, softwareVersion.Major, softwareVersion.Minor, softwareVersion.Build));
            services.AddTransient<IStartupTask, LoggerStartupTask>();
            services.AddSingleton<IFirebaseService, FirebaseService>((provider) => new FirebaseService(provider, "./connect-notification-9314f-firebase-adminsdk-m70fo-6cfc9a1076.json"));
            services.AddInMemoryEventServices();
        }

        public static void AddCacheServices(this IServiceCollection services)
        {
            services.AddSingleton<ICacheZaptoService<Sensor>, CacheZaptoService<Sensor>>();
            services.AddSingleton<ICacheZaptoService<Room>, CacheZaptoService<Room>>();
            services.AddSingleton<ICacheZaptoService<ConnectedObject>, CacheZaptoService<ConnectedObject>>();
            services.AddSingleton<ICacheZaptoService<Notification>, CacheZaptoService<Notification>>();
            services.AddSingleton<ICacheZaptoService<Model.Configuration>, CacheZaptoService<Model.Configuration>>();
            services.AddSingleton<ICacheZaptoService<Program>, CacheZaptoService<Program>>();
            services.AddSingleton<ICacheZaptoService<Condition>, CacheZaptoService<Condition>>();
            services.AddSingleton<ICacheZaptoService<Plug>,  CacheZaptoService<Plug>>();
            services.AddSingleton<ICacheZaptoService<OperationRange>, CacheZaptoService<OperationRange>>();
            services.AddSingleton<ICacheZaptoService<Location>, CacheZaptoService<Location>>();
        }
    }
}

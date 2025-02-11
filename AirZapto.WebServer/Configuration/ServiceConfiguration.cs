using AirZapto.Application;
using AirZapto.Data;
using AirZapto.Data.Database;
using AirZapto.Model;
using AirZapto.WebServer.Services;
using Framework.Common.Services;
using Framework.Data.Services;
using Framework.Infrastructure.Abstractions;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace AirZapto.WebServices.Configuration
{
    public static class ServiceConfiguration
	{
		public static void AddServices(this IServiceCollection services, IConfiguration configuration)
		{
            services.AddSecretService(configuration);
            services.AddJsonFileService();
			services.AddSupervisors();
			services.AddRepositories(AirZaptoConstants.ConnectionStringAirZaptotKey, AirZaptoConstants.ServerTypeAirZaptoKey);
            services.AddSingleton<CacheSignal>();
            services.AddApplicationAirZaptoServices();
            services.AddSingleton<IDatabaseService, AirZaptoDatabaseService>(provider => new AirZaptoDatabaseService(provider, AirZaptoConstants.ConnectionStringAirZaptotKey, AirZaptoConstants.ServerTypeAirZaptoKey));
            services.AddSingleton<IWebSocketService, WebSocketService>();
            services.AddSingleton<IWSMessageManager, SensorMessageManager>(); 
			services.AddSingleton<IHostedService, SensorConnectionService>();
            services.AddSingleton<IHostedService, ProcessingDataService>();
            services.AddSingleton<IHostedService, DailyService>();

            Version softwareVersion = new Version(configuration["Version"] ?? "0.0.0");
            services.AddTransient<IStartupTask, CreateDatabaseStartupTask>((provider) => new CreateDatabaseStartupTask(provider, softwareVersion.Major, softwareVersion.Minor, softwareVersion.Build));
            services.AddTransient<IStartupTask, LoggerStartupTask>();
            services.AddCacheServices();
        }

        public static void AddCacheServices(this IServiceCollection services)
        {
            services.AddSingleton<ICacheZaptoService<Sensor>, CacheZaptoService<Sensor>>();
        }
    }
}

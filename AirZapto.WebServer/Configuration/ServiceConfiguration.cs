﻿using AirZapto.Application;
using AirZapto.Data;
using AirZapto.Data.Database;
using AirZapto.Model;
using AirZapto.WebServer.Services;
using Framework.Common.Services;
using Framework.Data.Abstractions;
using Framework.Data.Services;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AirZapto.WebServices.Configuration
{
    public static class ServiceConfiguration
	{
		public static void AddServices(this IServiceCollection services)
		{ 
			services.AddJsonFileService();
			services.AddSupervisors();
			services.AddRepositories();
            services.AddSingleton<CacheSignal>();
            services.AddApplicationAirZaptoServices();
			services.AddSingleton<WebSockerService>();
            services.AddSingleton<IDatabaseService, AirZaptoDatabaseService>();
            services.AddTransient<IWSMessageManager, SensorMessageManager>(); 
			services.AddSingleton<IHostedService, SensorConnectionService>();
            services.AddSingleton<IHostedService, ProcessingDataService>();
            services.AddSingleton<IHostedService, DailyService>();
            services.AddTransient<IStartupTask, CreateDatabaseStartupTask>();
            services.AddTransient<IStartupTask, LoggerStartupTask>();
            services.AddCacheServices();
            services.AddTransient<IKeyVaultService, KeyVaultService>();
        }

        public static void AddCacheServices(this IServiceCollection services)
        {
            services.AddSingleton<ICacheZaptoService<Sensor>, CacheZaptoService<Sensor>>();
        }
    }
}

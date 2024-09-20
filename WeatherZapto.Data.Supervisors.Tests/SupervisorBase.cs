using Framework.Common.Services;
using Framework.Data.Abstractions;
using Framework.Data.Services;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using WeatherZapto.Data.Repository;
using WeatherZapto.Data.Services;
using WeatherZapto.WebServer.Services;

namespace WeatherZapto.Data.Supervisors.Tests
{
    public abstract class SupervisorBase : IAsyncLifetime
    {
        #region Properties
        protected IHost? HostApplication { get; set; } = null;
        private readonly PostgreSqlContainer Container = new PostgreSqlBuilder()
                                                                .WithImage("postgres:14.7")
                                                                .WithDatabase("weatherZaptoDb")
                                                                .WithUsername("postgres")
                                                                .WithPassword("postgres")
                                                                .WithCleanUp(true)
                                                                .Build();
        #endregion

        #region Constructor
        public SupervisorBase()
        {
           
        }
        #endregion

        #region Methods
        protected void CreateHost()
        {
            this.HostApplication = new HostBuilder().ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"ConnectionStrings:DefaultConnection", $"{Container.GetConnectionString()}"},
                    {"ConnectionStrings:ServerType", "PostGreSQL"}
                });
            }).ConfigureServices((context, services) =>
            {
                services.AddRepositories("ConnectionStringWeather", "ServerTypeWeather");
                services.AddSecretService(context.Configuration);
                services.AddSingleton<IDatabaseService, WeatherZaptoDatabaseService>(provider => new WeatherZaptoDatabaseService(provider, "ConnectionStringWeather", "ServerTypeWeather"));
                services.AddTransient<IStartupTask, CreateDatabaseStartupTask>();
                services.AddTransient<ICleanTask, DropDatabaseStartupTask>();
                services.AddTransient<IStartupTask, LoggerStartupTask>();
                services.AddTransient<ISupervisorVersion, SupervisorVersion>();
                services.AddSingleton<CacheSignal>();
                services.AddMemoryCache();

            })
           .Build();
        }

        protected virtual async Task Initialyse()
        {
            if (this.HostApplication != null)
            {
                var cleanTasks = this.HostApplication.Services.GetServices<ICleanTask>();
                foreach (var task in cleanTasks)
                {
                    await task.Execute();
                }

                var startupTasks = this.HostApplication.Services.GetServices<IStartupTask>();
                foreach (var task in startupTasks)
                {
                    await task.Execute();
                }
            }
        }

        public async Task InitializeAsync() => await Container.StartAsync();

        public async Task DisposeAsync() => await Container.DisposeAsync();
        #endregion
    }
}

using AirZapto.Data.Database;
using AirZapto.Data.Services;
using AirZapto.WebServer.Services;
using Framework.Data.Abstractions;
using Framework.Data.Services;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AirZapto.Data.Supervisors.Tests
{
    public abstract class SupervisorBase
    {
        #region Properties
        protected IHost HostApplication { get; }

        #endregion

        #region Constructor
        public SupervisorBase()
        {
            this.HostApplication = new HostBuilder().ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"ConnectionStrings:DefaultConnection", "Data Source=.\\airZaptoDb.db3;"},
                    {"ConnectionStrings:ServerType", "Sqlite"}
                });
            }).ConfigureServices((context, services) =>
            {
                services.AddRepositories();
                services.AddSingleton<IDatabaseService, AirZaptoDatabaseService>(provider => new AirZaptoDatabaseService(provider));
                services.AddTransient<IStartupTask, CreateDatabaseStartupTask>();
                services.AddTransient<ICleanTask, DropDatabaseStartupTask>();
                services.AddTransient<IStartupTask, LoggerStartupTask>();
                services.AddTransient<ISupervisorVersion, SupervisorVersion>();
                services.AddSupervisors();
            })
            .Build();
        }
        #endregion

        #region Methods
        protected virtual async Task Initialyse()
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
        #endregion
    }
}

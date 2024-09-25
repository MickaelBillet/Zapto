using Connect.Data.Database;
using Connect.Data.Repository;
using Connect.WebServer.Services;
using Framework.Data.Abstractions;
using Framework.Data.Services;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Connect.Data.Supervisors.Tests
{
    public abstract class SupervisorBase
    {
        #region Properties
        protected IHost? HostApplication { get; set; } = null;
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
                    {"ConnectionStrings:DefaultConnection", $"Data Source=.\\connectDb.db3;"},
                    {"ConnectionStrings:ServerType", "Sqlite"},
                    {"Cache",  "0"}
                });
            }).ConfigureServices((context, services) =>
            {
                services.AddRepositories();
                services.AddSingleton<IDatabaseService, ConnectDatabaseService>(provider => new ConnectDatabaseService(provider));
                services.AddTransient<IStartupTask, CreateDatabaseStartupTask>();
                services.AddTransient<ICleanTask, DropDatabaseStartupTask>();
                services.AddTransient<IStartupTask, LoggerStartupTask>();
                services.AddSupervisors();
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
        #endregion
    }
}

using AirZapto.Data.Database;
using AirZapto.Data.Services;
using AirZapto.Data.Services.Repositories;
using AirZapto.WebServer.Services;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using Framework.Data.Services;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Threading.Tasks;

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

                services.AddSingleton<IDatabaseService, AirZaptoDatabaseService>();
                services.AddTransient<IStartupTask, DatabaseStartupTask>();
                services.AddTransient<IStartupTask, LoggerStartupTask>();
                services.AddTransient<ISupervisorVersion, SupervisorVersion>();
            })
            .Build();
        }
        #endregion

        #region Methods
        public async Task Initialyse()
        {
            var startupTasks = this.HostApplication.Services.GetServices<IStartupTask>();
            foreach (var task in startupTasks)
            {
                await task.Execute();
            }
        }
        #endregion
    }
}

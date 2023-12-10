using Framework.Core.Base;
using Framework.Data.Abstractions;
using Framework.Data.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AirZapto.Data.Database
{
    public sealed class AirZaptoDatabaseService : DatabaseService
    {
        #region Services
        #endregion

        #region Constructor
        public AirZaptoDatabaseService(IDataContextFactory dataContextFactory, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base(dataContextFactory, serviceScopeFactory, configuration)
        {
        }
        #endregion

        #region Methods
        protected override async Task<bool> UpgradeDatabaseAsync()
        {
            bool res = true;
            using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
            {
                ISupervisorVersion supervisor = scope.ServiceProvider.GetRequiredService<ISupervisorVersion>();
                Version dbVersion = await supervisor.GetVersionAsync();
                Log.Information($"dbVersion : {dbVersion}");

                if (this.Configuration != null)
                {
                    Version softwareVersion = new Version(this.Configuration["Version"] ?? "0.0.0");
                    Log.Information($"softwareVersion : {softwareVersion}");
                    if (softwareVersion > dbVersion)
                    {
                    }
                    ResultCode result = await supervisor.UpdateVersionAsync(softwareVersion.Major, softwareVersion.Minor, softwareVersion.Build);
                    res = (result == ResultCode.Ok) ? true : false;
                }
            }
            return res;
        }

        protected override async Task FeedDataAsync()
        {
            using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
            {
                ISupervisorVersion supervisor = scope.ServiceProvider.GetRequiredService<ISupervisorVersion>();
                await supervisor.AddVersionAsync();
            }
        }

        #endregion
    }
}

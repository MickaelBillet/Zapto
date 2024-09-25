using AirZapto.Data.Services;
using Framework.Core.Base;
using Framework.Data.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AirZapto.Data.Database
{
    public sealed class AirZaptoDatabaseService : DatabaseService
    {
        #region Constructor
        public AirZaptoDatabaseService(IServiceProvider serviceProvider, string connectionStringKey, string serverTypeKey) : base(serviceProvider, connectionStringKey, serverTypeKey)
        {
        }

        public AirZaptoDatabaseService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        #endregion

        #region Methods
        protected override async Task<bool> UpgradeDatabaseAsync()
        {
            bool res = true;
            if (this.ServiceScopeFactory != null)
            {
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
                        if (result == ResultCode.ItemNotFound)
                        {
                            result = await supervisor.AddVersionAsync();
                        }
                        res = (result == ResultCode.Ok) ? true : false;
                    }
                }
            }
            return res;
        }

        protected override async Task FeedDataAsync()
        {
            if (this.ServiceScopeFactory != null)
            {
                using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
                {
                    ISupervisorVersion supervisor = scope.ServiceProvider.GetRequiredService<ISupervisorVersion>();
                    await supervisor.AddVersionAsync();
                }
            }
        }
        #endregion
    }
}

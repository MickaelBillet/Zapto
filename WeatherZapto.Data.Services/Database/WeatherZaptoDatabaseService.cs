using Framework.Core.Base;
using Framework.Data.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace WeatherZapto.Data.Services
{
    public sealed class WeatherZaptoDatabaseService : DatabaseService
    {
        #region Properties
        #endregion

        #region Constructor
        public WeatherZaptoDatabaseService(IServiceProvider serviceProvider) : base(serviceProvider)
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
                Version dbVersion = await supervisor.GetVersion();
                Log.Information($"dbVersion : {dbVersion}");

                if (this.Configuration != null)
                {
                    Version softwareVersion = new Version(this.Configuration["Version"] ?? "0.0.0");
                    Log.Information($"softwareVersion : {softwareVersion}");
                    if (softwareVersion > dbVersion)
                    {
                    }
                    ResultCode result = await supervisor.UpdateVersion(softwareVersion.Major, softwareVersion.Minor, softwareVersion.Build);
                    if (result == ResultCode.ItemNotFound)
                    {
                        result = await supervisor.AddVersion();
                    }
                    res = (result == ResultCode.Ok);
                }
            }
            return res;
        }

        protected override async Task FeedDataAsync()
        {
            using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
            {
                await scope.ServiceProvider.GetRequiredService<ISupervisorVersion>().AddVersion();
            }
        }

        #endregion
    }
}

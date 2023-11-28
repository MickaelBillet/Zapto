using Framework.Core.Base;
using Framework.Data.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace WeatherZapto.Data.Services
{
    public sealed class WeatherZaptoDatabaseService : DatabaseService
    {
        #region Properties
        private IServiceScopeFactory ServiceScopeFactory { get; }
        #endregion

        #region Constructor
        public WeatherZaptoDatabaseService(IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base(serviceProvider, serviceScopeFactory, configuration)
        {
            this.ServiceScopeFactory = serviceScopeFactory;
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
                    res = (result == ResultCode.Ok) ? true : false;
                }
            }
            return res;
        }

        #endregion
    }
}

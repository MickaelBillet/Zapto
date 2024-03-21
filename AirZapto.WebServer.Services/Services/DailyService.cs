using AirZapto.Data;
using AirZapto.Data.Services;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AirZapto.WebServer.Services
{
    public class DailyService : CronScheduledService
    {
        #region Properties 

        protected override string Schedule => "59 23 * * *";

        //protected override string Schedule => "*/1 * * * *";

        private IConfiguration Configuration { get; }

        #endregion

        #region Constructor

        public DailyService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base(serviceScopeFactory)
        {
            this.Configuration = configuration;
        }

        #endregion

        #region Methods

        public override async Task ProcessInScope(IServiceScope scope)
        {
            ISupervisorSensorData supervisor = scope.ServiceProvider.GetRequiredService<ISupervisorSensorData>();
            ResultCode result = await supervisor.DeleteSensorDataAsync(new TimeSpan(48,0,0));
            if (result != ResultCode.Ok)
            {
                Log.Error("Delete SensorData Error");
            }
        }

        #endregion
    }
}

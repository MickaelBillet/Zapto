using AirZapto.Data;
using AirZapto.Data.Services;
using AirZapto.Model;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AirZapto.WebServer.Services
{
    public class ProcessingDataService : ScheduledService
    {
        #region Properties

        private IConfiguration Configuration { get; }

        #endregion

        #region Constructor

        public ProcessingDataService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base(serviceScopeFactory, Convert.ToInt32(configuration["ProcessingDataPeriod"]))
        {
            this.Configuration = configuration;
        }

        #endregion

        #region Methods 

        /// <summary>
        /// Process the data in a scoped service which is launched at the startup of the app
        /// </summary>
        /// <returns></returns>
        public override async Task ProcessInScope(IServiceScope scope)
        {
            Log.Information("ProcessingDataService.ProcessInScope");

            try
            {
                await this.ProcessSensorStatus(scope);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }
        }

        private async Task ProcessSensorStatus(IServiceScope scope)
        {
            ISupervisorSensor supervisor = scope.ServiceProvider.GetRequiredService<ISupervisorFactorySensor>().CreateSupervisor();
            (ResultCode result, IEnumerable<Sensor>? sensors) result = await supervisor.GetSensorsAsync();

            //30 minutes
            int period = 30;

            if ((this.Configuration != null) && (this.Configuration["ValidityPeriod"] != null))
            {
                int.TryParse(this.Configuration["ValidityPeriod"], out period);
            }

            if ((result.result == ResultCode.Ok) && (result.sensors != null))  
            {
                List<Sensor> sensors = result.sensors.ToList();
                foreach (Sensor sensor in sensors)
                {
                    if (sensor.Date + new TimeSpan(0, period, 0) < Clock.Now)
                    {
                        sensor.IsRunning = RunningStatus.UnHealthy;
                    }
                    else
                    {
                        sensor.IsRunning = RunningStatus.Healthy;
                    }

                    await supervisor.UpdateSensorAsync(sensor);
                }
            }
        }

        #endregion
    }
}

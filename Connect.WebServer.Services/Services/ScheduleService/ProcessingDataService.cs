using Connect.Application;
using Connect.Data;
using Connect.Model;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Connect.WebServer.Services.Services.ScheduleService
{
    public class ProcessingDataService : ScheduledService
    {
        private const int DefaultSensorDataValidityPeriod = 30; //30 minutes

        #region Properties

        private IConfiguration Configuration { get; }

        #endregion

        #region Constructor

        public ProcessingDataService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base(serviceScopeFactory, Convert.ToInt32(configuration["ProcessingDataPeriod"]))
        {
            Configuration = configuration;
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
                ISupervisorPlug supervisorPlug = scope.ServiceProvider.GetRequiredService<ISupervisorPlug>();
                ISupervisorSensor supervisorSensor = scope.ServiceProvider.GetRequiredService<ISupervisorSensor>();
                ISendCommandService sendCommandService = scope.ServiceProvider.GetRequiredService<ISendCommandService>();

                await ProcessPlugStatus(supervisorPlug, sendCommandService);
                await ProcessSensorStatus(supervisorSensor);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }
        }

        private async Task ProcessPlugStatus(ISupervisorPlug supervisorPlug, ISendCommandService sendCommandService)
        {
            List<Plug> plugs = (await supervisorPlug.GetPlugs()).ToList();
            foreach (Plug plug in plugs)
            {
                await sendCommandService.Execute(plug);                    
            }
        }

        private async Task ProcessSensorStatus(ISupervisorSensor supervisorSensor)
        {
            List<Sensor> sensors = (await supervisorSensor.GetSensors()).ToList();

            int period = DefaultSensorDataValidityPeriod;

            if (Configuration != null && Configuration["ValidityPeriod"] != null)
            {
                int.TryParse(Configuration["ValidityPeriod"], out period);
            }

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

                await supervisorSensor.UpdateSensor(sensor);
            }
        }

        #endregion
    }
}

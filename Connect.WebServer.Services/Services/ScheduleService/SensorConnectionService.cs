using Connect.Application;
using Connect.Data;
using Connect.Model;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Connect.WebServer.Services
{
    public class SensorConnectionService : ScheduledService
    {
        #region Properties

        #endregion

        #region Constructor

        public SensorConnectionService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base(serviceScopeFactory, Convert.ToInt32(configuration["ConnectionPeriod"]))
        {

        }

        #endregion

        #region Methods 

        /// <summary>
        /// Process the data in a scoped service which is launched at the startup of the app
        /// </summary>
        /// <returns></returns>
        public override async Task ProcessInScope(IServiceScope scope)
        {
            Log.Information("SensorConnectionService.ProcessInScope");

            try
            {
                IApplicationSensorServices applicationSensorServices = scope.ServiceProvider.GetRequiredService<IApplicationSensorServices>();
                ISupervisorSensor supervisor = scope.ServiceProvider.GetRequiredService<ISupervisorSensor>();
                IEnumerable<Sensor> sensors = await supervisor.GetSensors();
                foreach (Sensor sensor in sensors)
                {
                    await applicationSensorServices.Notify(sensor);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }
        }

        #endregion
    }
}

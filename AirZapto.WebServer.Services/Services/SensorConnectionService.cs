using AirZapto.Application;
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
    public class SensorConnectionService : ScheduledService
    {
        #region Properties

        private IConfiguration Configuration { get; }

        #endregion

        #region Constructor

        public SensorConnectionService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base(serviceScopeFactory, Convert.ToInt32(configuration["ConnectionPeriod"]))
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
            Log.Information("SensorConnectionService");

            try
            {
                ISupervisorCacheSensor supervisor = scope.ServiceProvider.GetRequiredService<ISupervisorCacheSensor>();
                IApplicationSensorServices applicationSensorServices = scope.ServiceProvider.GetRequiredService<IApplicationSensorServices>();
                (ResultCode code, IEnumerable<Sensor>? sensors) result = await supervisor.GetSensorsAsync();
                bool isConnected = false;

                if ((result.code == ResultCode.Ok) && (result.sensors != null)) 
                {
                    foreach (Sensor sensor in result.sensors)
                    {
                        isConnected = await applicationSensorServices.SendCommandAsync(sensor, Command.Connection);
                        if (isConnected == false)
                        {
                            sensor.Mode = SensorMode.Initial;
                            await supervisor.UpdateSensorAsync(sensor);
                        }
                        else
                        {
                            Log.Information($"Sensor found : {sensor.IdSocket}" );
                        }
                    }
                }
            }
            catch(Exception) 
            {
                throw;
            }
        }

        #endregion
    }
}

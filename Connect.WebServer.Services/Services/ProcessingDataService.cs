using Connect.Application;
using Connect.Data;
using Connect.Model;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Connect.WebServer.Services
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
                ISupervisorRoom supervisorRoom = scope.ServiceProvider.GetRequiredService<ISupervisorRoom>();
                ISupervisorPlug supervisorPlug = scope.ServiceProvider.GetRequiredService<ISupervisorPlug>();
                ISupervisorSensor supervisorSensor = scope.ServiceProvider.GetRequiredService<ISupervisorSensor>();

                IApplicationPlugServices applicationPlugServices = scope.ServiceProvider.GetRequiredService<IApplicationPlugServices>();

                await this.ProcessPlugStatus(supervisorPlug, supervisorRoom, applicationPlugServices);
                await this.ProcessSensorStatus(supervisorSensor);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }
        }

        private async Task ProcessPlugStatus(ISupervisorPlug supervisorPlug, ISupervisorRoom supervisorRoom, IApplicationPlugServices applicationPlugServices)
        {                
            List<Plug> plugs = (await supervisorPlug.GetPlugs()).ToList();
            foreach (Plug plug in plugs)
            {
                ResultCode resultCode = ResultCode.CouldNotUpdateItem;
                Room room = await supervisorRoom.GetRoomFromPlugId(plug.Id);
                if (room != null)
                {
                    //Get order before the update
                    int previousOrder = plug.Order;

                    //Update the order with all the data
                    plug.UpdateOrder(room.Humidity, room.Temperature);

                    //We send the command to the Arduino when the order changes
                    if (plug.Order != previousOrder)
                    {
                        //Send Command to Arduino
                        if (await applicationPlugServices.SendCommandAsync(plug) <= 0)
                        {
                            //If we cannot send the command to the Arduino, we keep the previous value
                            plug.Order = previousOrder;
                        }
                    }

                    Log.Information("ProcessingDataService.ProcessInScope " + "Id : " + plug.Id);
                    Log.Information("ProcessingDataService.ProcessInScope " + "OnOff : " + plug.OnOff);
                    Log.Information("ProcessingDataService.ProcessInScope " + "Status : " + plug.Status);
                    Log.Information("ProcessingDataService.ProcessInScope " + "Order : " + plug.Order);

                    //Update the working duration
                    plug.ComputeWorkingDuration();

                    //Send Status to the clients app (Mobile, Web...)
                    await applicationPlugServices.SendStatusToClientAsync(room.LocationId, plug);
                    resultCode = await supervisorPlug.UpdatePlug(plug);
                }
            }
        }

        private async Task ProcessSensorStatus(ISupervisorSensor supervisorSensor)
        {
            List<Sensor> sensors = (await supervisorSensor.GetSensors()).ToList();

            //30 minutes
            int period = 30;

            if ((this.Configuration != null) && (this.Configuration["ValidityPeriod"] != null))
            {
                int.TryParse(this.Configuration["ValidityPeriod"], out period);
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

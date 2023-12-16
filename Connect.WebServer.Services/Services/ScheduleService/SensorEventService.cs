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
    public class SensorEventService : ScheduledService
    {
        #region Properties
        #endregion

        #region Constructor
        public SensorEventService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base(serviceScopeFactory, 10)
        {
        }
        #endregion

        #region Methods 
        /// <summary>
        /// Scoped service which is launched at the startup of the app
        /// Read the Sensor Event
        /// </summary>
        /// <returns></returns>
        public override async Task ProcessInScope(IServiceScope scope)
        {
            Log.Information("SensorEventService.ProcessInScope");
            ResultCode resultCode = ResultCode.ItemNotFound;

            try
            {
                ISupervisorRoom supervisorRoom = scope.ServiceProvider.GetRequiredService<ISupervisorRoom>();
                ISupervisorSensor supervisorSensor = scope.ServiceProvider.GetRequiredService<ISupervisorSensor>();
                IApplicationSensorServices applicationSensorServices = scope.ServiceProvider.GetRequiredService<IApplicationSensorServices>();
                SensorEvent? @event = await applicationSensorServices.ReceiveEventAsync();
                if (@event != null)
                {
                    Sensor sensor = await supervisorSensor.GetSensor(@event.Type, @event.Channel);
                    if (sensor != null)
                    {
                        sensor.LeakDetected = @event.Leak;
                        //Update in database the data of the sensor
                        (resultCode, sensor) = await supervisorSensor.UpdateSensor(sensor);
                        if (resultCode == ResultCode.Ok)
                        {
                            //Test if the sensor is linked with a room or a connectedobject
                            if (string.IsNullOrEmpty(sensor.RoomId) == false)
                            {
                                await ProcessRoomData(applicationSensorServices, supervisorRoom, sensor);
                            }
                        }
                        else
                        {
                            Log.Error("SensorEventService.ProcessInScope Error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }
        }

        private async Task ProcessRoomData(IApplicationSensorServices applicationSensorServices, ISupervisorRoom supervisorRoom, Sensor sensor)
        {
            Room? room = await supervisorRoom.GetRoom(sensor.RoomId);
            if (room != null)
            {
                //Send event to the client apps
                await applicationSensorServices.SendEventToClientAsync(room.LocationId, sensor);

                //Send Notification to the Firebase app
                await applicationSensorServices.NotifySensorLeak(room.LocationId, room);
            }
        }

        #endregion
    }
}

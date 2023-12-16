using Connect.Application;
using Connect.Data;
using Connect.Model;
using Framework.Core;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Connect.WebServer.Services
{
    public class SensorDataService : ScheduledService
    {
        private const double MARGIN = 0.5;
        private const int MEASURES_COUNT = 10;

        #region Properties
        private IConfiguration Configuration { get; }
        #endregion

        #region Constructor

        public SensorDataService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base(serviceScopeFactory, 10)
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
            Log.Information("SensorDataService.ProcessInScope");
            ResultCode resultCode = ResultCode.ItemNotFound;

            try
            {
                ISupervisorRoom supervisorRoom = scope.ServiceProvider.GetRequiredService<ISupervisorRoom>();
                ISupervisorSensor supervisorSensor = scope.ServiceProvider.GetRequiredService<ISupervisorSensor>();
                ISupervisorNotification supervisorNotification = scope.ServiceProvider.GetRequiredService<ISupervisorNotification>();
                ISupervisorConnectedObject supervisorConnectedObject = scope.ServiceProvider.GetRequiredService<ISupervisorConnectedObject>();
                ISupervisorOperatingData supervisorOperatingData = scope.ServiceProvider.GetRequiredService<ISupervisorOperatingData>();
                IApplicationConnectedObjectServices applicationConnectedObjectServices = scope.ServiceProvider.GetRequiredService<IApplicationConnectedObjectServices>();
                IApplicationRoomServices applicationRoomServices = scope.ServiceProvider.GetRequiredService<IApplicationRoomServices>();
                IApplicationSensorServices applicationSensorServices = scope.ServiceProvider.GetRequiredService<IApplicationSensorServices>();
                SensorData? data = await applicationSensorServices.ReceiveDataAsync();
                if (data != null)
                {
                    Sensor sensor = await supervisorSensor.GetSensor(data.Type, data.Channel);
                    if (sensor != null)
                    {
                        if (await this.CheckHumidityValue(supervisorOperatingData, data.Humidity, sensor.RoomId)
                            && await this.CheckTemperatureValue(supervisorOperatingData, data.Temperature, sensor.RoomId))
                        {
                            sensor.ProcessData(data.Temperature, data.Humidity, data.Pressure);
                            (resultCode, sensor) = await supervisorSensor.UpdateSensor(sensor);
                            if (resultCode == ResultCode.Ok)
                            {
                                //Test if the sensor is linked with a room or a connectedobject
                                if (string.IsNullOrEmpty(sensor.RoomId) == false)
                                {
                                    await ProcessRoomData(supervisorRoom, supervisorNotification, applicationRoomServices, sensor);
                                }
                                else if (string.IsNullOrEmpty(sensor.ConnectedObjectId) == false)
                                {
                                    await ProcessConnectedObjectData(supervisorRoom, supervisorConnectedObject, applicationConnectedObjectServices, supervisorNotification, sensor);
                                }
                            }
                            else
                            {
                                Log.Error("SensorDataService.ProcessInScope Error");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }
        }

        private async Task ProcessRoomData(ISupervisorRoom supervisorRoom,
                                            ISupervisorNotification supervisorNotification,
                                            IApplicationRoomServices applicationRoomServices,
                                            Sensor sensor)
        {
            Room room = await supervisorRoom.GetRoom(sensor.RoomId);
            if (room != null)
            {
                int validityPeriod = 0;

                if (Configuration != null && Configuration["ValidityPeriod"] != null)
                {
                    int.TryParse(Configuration["ValidityPeriod"], out validityPeriod);
                }

                //Compute the data (Temperature, Humidity, Pressure) of the room
                room.ComputeData(validityPeriod);

                //Send Data to the client app (Mobile, Web...)
                await applicationRoomServices.SendDataToClientAsync(room.LocationId, room);

                //Send Notification to the Firebase app
                await applicationRoomServices.NotifiyRoomCondition(room.LocationId, room.NotificationsList, room);
                await supervisorNotification.UpdateNotifications(room.NotificationsList);
                await supervisorRoom.UpdateRoom(room);
            }
        }

        private async Task ProcessConnectedObjectData(ISupervisorRoom supervisorRoom,
                                                        ISupervisorConnectedObject supervisorConnectedObject,
                                                        IApplicationConnectedObjectServices applicationConnectedObjectServices,
                                                        ISupervisorNotification supervisorNotification,
                                                        Sensor sensor)
        {
            ConnectedObject connectedObject = await supervisorConnectedObject.GetConnectedObject(sensor.ConnectedObjectId, true);
            if (connectedObject != null)
            {
                Room room = await supervisorRoom.GetRoom(connectedObject.RoomId);
                if (room != null)
                {
                    //Send data to the client app
                    await applicationConnectedObjectServices.SendDataToClientAsync(room.LocationId, connectedObject);

                    //Send Notification to the Firebase app
                    await applicationConnectedObjectServices.NotifiyConnectedObjectCondition(connectedObject.NotificationsList, room, connectedObject);
                    await supervisorNotification.UpdateNotifications(connectedObject.NotificationsList);
                }
            }
        }

        private async Task<bool> CheckTemperatureValue(ISupervisorOperatingData supervisorOperatingData, float temperature, string roomId)
        {
            bool isValid = false;
            IEnumerable<(double?, DateTime)> temperatures = (await supervisorOperatingData.GetRoomOperatingDataOfDay(roomId, DateTime.Today)).Select(x => (x.Temperature, x.Date));

            //We begin to delete the false measures after the first MEASURES_COUNT measures
            if (temperatures.Count() >= MEASURES_COUNT)
            {
                (double? temp, DateTime time) median = ZaptoMath.Median(temperatures.TakeLast(MEASURES_COUNT));
                if (median.temp != null && Math.Abs(temperature - median.temp.Value) < median.temp * MARGIN)
                {
                    isValid = true;
                }

                //We delete the false measures from the first MEASURES_COUNT measures
                if (temperatures.Count() == MEASURES_COUNT)
                {
                    this.DeleteFalseMeasure(supervisorOperatingData, median.temp, temperatures, roomId);
                }
            }
            else
            {
                isValid = true;
            }
            return isValid;
        }

        private async Task<bool> CheckHumidityValue(ISupervisorOperatingData supervisorOperatingData, float humidity, string roomId)
        {
            bool isValid = false;
            IEnumerable<(double?, DateTime)> humidities = (await supervisorOperatingData.GetRoomOperatingDataOfDay(roomId, DateTime.Today)).Select(x => (x.Humidity, x.Date));

            //We begin to delete the false measures after the first MEASURES_COUNT measures
            if (humidities.Count() >= MEASURES_COUNT)
            {
                (double? hum, DateTime time) median = ZaptoMath.Median(humidities.TakeLast(MEASURES_COUNT));
                if (median.hum != null && Math.Abs(humidity - median.hum.Value) < median.hum * MARGIN)
                {
                    isValid = true;
                }

                //We delete the false measures from the first MEASURES_COUNT measures
                if (humidities.Count() == MEASURES_COUNT)
                {
                    this.DeleteFalseMeasure(supervisorOperatingData, median.hum, humidities, roomId);
                }
            }
            else
            {
                isValid = true;
            }
            return isValid;
        }

        private void DeleteFalseMeasure(ISupervisorOperatingData supervisorOperatingData, double? median, IEnumerable<(double?, DateTime)> values, string roomId)
        {
            List<DateTime> remove = new List<DateTime>();

            foreach ((double? temp, DateTime time) obj in values)
            {
                if (median != null && obj.temp != null && Math.Abs(obj.temp.Value - median.Value) > median * MARGIN)
                {
                    remove.Add(obj.time);
                }
            }

            remove.ForEach(async item => await supervisorOperatingData.DeleteOperationData(item, roomId));
        }
        #endregion
    }
}

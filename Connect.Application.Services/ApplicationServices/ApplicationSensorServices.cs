using Connect.Application.Infrastructure;
using Connect.Data;
using Connect.Model;
using Framework.Common.Services;
using Framework.Core;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    internal sealed class ApplicationSensorServices : IApplicationSensorServices
    { 
        private const double MARGIN = 0.5;
        private const int MEASURES_COUNT = 10;

        #region Services
        private ISensorService? SensorService { get; }
        private ISignalRConnectService? SignalRConnectService { get; }
        private IAlertService? AlertService { get; }
        private IServiceScopeFactory? ServiceScopeFactory { get; }
        private ISendMessageToArduinoService? SendMessageToArduino { get; }
        #endregion

        #region Constructor
        public ApplicationSensorServices(IServiceProvider serviceProvider)
        {
            this.SignalRConnectService = serviceProvider.GetService<ISignalRConnectService>();
            this.SensorService = serviceProvider.GetService<ISensorService>();
            this.AlertService = AlertServiceFactory.CreateAlerteService(serviceProvider, ServiceAlertType.Mail);
            this.ServiceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>();
            this.SendMessageToArduino = serviceProvider.GetService<ISendMessageToArduinoService>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Send the sensor configuration to the arduino server
        /// </summary>
        /// <returns></returns>
        public async Task<int?> Notify(Sensor sensor)
        {
            int? res = 0;
            string? data = sensor.SerializeSensorConfiguration();
            if ((this.SendMessageToArduino != null) && (string.IsNullOrEmpty(data) == false))
            {
                res = await this.SendMessageToArduino.Send(MessageArduino.Serialize(new MessageArduino()
                {
                    Header = ConnectConstants.SensorConfig,
                    Payload = data
                }));

                if (res > 0)
                {
                    Log.Warning("Notify Sensor : " + sensor.IpAddress);
                }
                else
                {
                    Log.Error("Error Notification : " + sensor.IpAddress);
                }
            }
            return res;
        }

        public async Task ReadData(SensorData? sensorData)
        {
            Log.Information("ApplicationSensorServices.ReadData");

            try
            {
                if (this.ServiceScopeFactory != null)
                {
                    using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
                    {
                        IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                        ISupervisorRoom supervisorRoom = scope.ServiceProvider.GetRequiredService<ISupervisorFactoryRoom>().CreateSupervisor(byte.Parse(configuration["Cache"]!));
                        ISupervisorSensor supervisorSensor = scope.ServiceProvider.GetRequiredService<ISupervisorFactorySensor>().CreateSupervisor(byte.Parse(configuration["Cache"]!));
                        ISupervisorNotification supervisorNotification = scope.ServiceProvider.GetRequiredService<ISupervisorFactoryNotification>().CreateSupervisor(byte.Parse(configuration["Cache"]!));
                        ISupervisorConnectedObject supervisorConnectedObject = scope.ServiceProvider.GetRequiredService<ISupervisorFactoryConnectedObject>().CreateSupervisor(byte.Parse(configuration["Cache"]!));
                        ISupervisorOperatingData supervisorOperatingData = scope.ServiceProvider.GetRequiredService<ISupervisorFactoryOperatingData>().CreateSupervisor();
                        IApplicationConnectedObjectServices applicationConnectedObjectServices = scope.ServiceProvider.GetRequiredService<IApplicationConnectedObjectServices>();
                        IApplicationRoomServices applicationRoomServices = scope.ServiceProvider.GetRequiredService<IApplicationRoomServices>();
                        if (sensorData != null)
                        {
                            ResultCode resultCode = ResultCode.ItemNotFound;
                            Sensor sensor = await supervisorSensor.GetSensor(sensorData.Type, sensorData.Channel);
                            if (sensor != null)
                            {
                                if (await this.CheckHumidityValue(supervisorOperatingData, sensorData.Humidity, sensor.RoomId)
                                    && await this.CheckTemperatureValue(supervisorOperatingData, sensorData.Temperature, sensor.RoomId))
                                {
                                    sensor.ProcessData(sensorData.Temperature, sensorData.Humidity, sensorData.Pressure);
                                    (resultCode, sensor) = await supervisorSensor.UpdateSensor(sensor);
                                    if (resultCode == ResultCode.Ok)
                                    {
                                        //Test if the sensor is linked with a room or a connectedobject
                                        if (string.IsNullOrEmpty(sensor.RoomId) == false)
                                        {
                                            await this.ProcessRoomData(configuration, supervisorRoom, supervisorNotification, applicationRoomServices, sensor);
                                        }
                                        else if (string.IsNullOrEmpty(sensor.ConnectedObjectId) == false)
                                        {
                                            await this.ProcessConnectedObjectData(supervisorRoom, supervisorConnectedObject, applicationConnectedObjectServices, supervisorNotification, sensor);
                                        }
                                    }
                                    else
                                    {
                                        Log.Error("ApplicationSensorServices.ReadData Error");
                                    }
                                }
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

        public async Task ReadEvent(SensorEvent? sensorEvent)
        {
            Log.Information("ApplicationSensorServices.ReadEvent");

            try
            {
                if (this.ServiceScopeFactory != null)
                {
                    using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
                    {
                        IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                        ISupervisorRoom supervisorRoom = scope.ServiceProvider.GetRequiredService<ISupervisorFactoryRoom>().CreateSupervisor(byte.Parse(configuration["Cache"]!));
                        ISupervisorSensor supervisorSensor = scope.ServiceProvider.GetRequiredService<ISupervisorFactorySensor>().CreateSupervisor(byte.Parse(configuration["Cache"]!));
                        if (sensorEvent != null)
                        {
                            Sensor sensor = await supervisorSensor.GetSensor(sensorEvent.Type, sensorEvent.Channel);
                            if (sensor != null)
                            {
                                ResultCode resultCode = ResultCode.ItemNotFound;
                                sensor.LeakDetected = sensorEvent.Leak;
                                //Update in database the data of the sensor
                                (resultCode, sensor) = await supervisorSensor.UpdateSensor(sensor);
                                if (resultCode == ResultCode.Ok)
                                {
                                    //Test if the sensor is linked with a room or a connectedobject
                                    if (string.IsNullOrEmpty(sensor.RoomId) == false)
                                    {
                                        await this.ProcessRoomData(supervisorRoom, sensor);
                                    }
                                }
                                else
                                {
                                    Log.Error("ApplicationSensorServices.ReadEvent Error");
                                }
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

        public async Task SendEventToClient(string locationId, Sensor sensor)
        {
            if (this.SignalRConnectService != null)
            {
                await this.SignalRConnectService.SendSensorStatusAsync(locationId, sensor);
                Log.Warning("SendEventToClient : " + sensor.Name + "/" + sensor.Channel + " LeakDetected");
            }
        }

        public async Task NotifySensorLeak(string locationId, Room room)
        {
            if (this.AlertService != null)
            {
                await this.AlertService.SendAlertAsync(locationId, "Fuite", "Fuite détectée dans la " + room.Name);
            }
        }

        private async Task ProcessRoomData(IConfiguration configuration,
                                            ISupervisorRoom supervisorRoom,
                                            ISupervisorNotification supervisorNotification,
                                            IApplicationRoomServices applicationRoomServices,
                                            Sensor sensor)
        {
            Room room = await supervisorRoom.GetRoom(sensor.RoomId);
            if (room != null)
            {
                int validityPeriod = 0;

                if (configuration != null && configuration["ValidityPeriod"] != null)
                {
                    int.TryParse(configuration["ValidityPeriod"], out validityPeriod);
                }

                //Compute the data (Temperature, Humidity, Pressure) of the room
                room.ComputeData(validityPeriod);

                //Send Data to the client app (Mobile, Web...)
                await applicationRoomServices.SendDataToClient(room.LocationId, room);

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
            ConnectedObject connectedObject = await supervisorConnectedObject.GetConnectedObject(sensor.ConnectedObjectId);
            if (connectedObject != null)
            {
                Room room = await supervisorRoom.GetRoom(connectedObject.RoomId);
                if (room != null)
                {
                    //Send data to the client app
                    await applicationConnectedObjectServices.SendDataToClient(room.LocationId, connectedObject);

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
        private async Task ProcessRoomData(ISupervisorRoom supervisorRoom, Sensor sensor)
        {
            Room? room = await supervisorRoom.GetRoom(sensor.RoomId);
            if (room != null)
            {
                //Send event to the client apps
                await this.SendEventToClient(room.LocationId, sensor);

                //Send Notification
                await this.NotifySensorLeak(room.LocationId, room);
            }
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

        public async Task<bool?> Leak(string? sensorId, int leakStatus)
        {
            return (this.SensorService != null) ? await this.SensorService.Leak(sensorId, leakStatus.ToString()) : null;
        }
        #endregion
    }
}
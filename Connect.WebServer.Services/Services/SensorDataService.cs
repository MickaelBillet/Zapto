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

        #region Properties
        private IConfiguration Configuration { get; }
        #endregion

        #region Constructor

        public SensorDataService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base(serviceScopeFactory, 10)
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
            Log.Information("SensorDataService.ProcessInScope");
            ResultCode resultCode = ResultCode.ItemNotFound;

            try
            {
                ISupervisorSensor supervisorSensor = scope.ServiceProvider.GetRequiredService<ISupervisorSensor>();
				IApplicationSensorServices applicationSensorServices = scope.ServiceProvider.GetRequiredService<IApplicationSensorServices>();
				SensorData? data = await applicationSensorServices.ReceiveDataAsync();
				if (data != null)
				{
					Sensor sensor = await supervisorSensor.GetSensor(data.Type, data.Channel);
					if (sensor != null)
					{
						if ((await this.CheckHumidityValue(scope, data.Humidity, sensor.RoomId)) 
							&& (await this.CheckTemperatureValue(scope, data.Temperature, sensor.RoomId)))
						{
							sensor.ProcessData(data.Temperature, data.Humidity, data.Pressure);
							(resultCode, sensor) = await supervisorSensor.UpdateSensor(sensor);
							if (resultCode == ResultCode.Ok)
							{
								//Test if the sensor is linked with a room or a connectedobject
								if (string.IsNullOrEmpty(sensor.RoomId) == false)
								{
									await this.ProcessRoomData(scope, sensor);
								}
								else if (string.IsNullOrEmpty(sensor.ConnectedObjectId) == false)
								{
									await this.ProcessConnectedObjectData(scope, sensor);    
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
            catch(Exception ex)
            {
                Log.Fatal(ex.Message);
            }
        }

		private async Task ProcessRoomData(IServiceScope scope, Sensor sensor)
		{
            ISupervisorRoom supervisorRoom = scope.ServiceProvider.GetRequiredService<ISupervisorRoom>();
            ISupervisorNotification supervisorNotification = scope.ServiceProvider.GetRequiredService<ISupervisorNotification>();
            IApplicationRoomServices applicationRoomServices = scope.ServiceProvider.GetRequiredService<IApplicationRoomServices>();
            Room room = await supervisorRoom.GetRoom(sensor.RoomId);
            if (room != null)
            {
                int validityPeriod = 0;

                if ((this.Configuration != null) && (this.Configuration["ValidityPeriod"] != null))
                {
                    int.TryParse(this.Configuration["ValidityPeriod"], out validityPeriod);
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

        private async Task ProcessConnectedObjectData(IServiceScope scope, Sensor sensor)
        {
            ISupervisorRoom supervisorRoom = scope.ServiceProvider.GetRequiredService<ISupervisorRoom>();
            ISupervisorConnectedObject supervisorConnectedObject = scope.ServiceProvider.GetRequiredService<ISupervisorConnectedObject>();
            IApplicationConnectedObjectServices applicationConnectedObjectServices = scope.ServiceProvider.GetRequiredService<IApplicationConnectedObjectServices>();
            ISupervisorNotification supervisorNotification = scope.ServiceProvider.GetRequiredService<ISupervisorNotification>();
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

        private async Task<bool> CheckTemperatureValue(IServiceScope scope, float temperature, string roomId)
		{
			bool isValid = false;
			IEnumerable<(double?, DateTime)> temperatures = (await scope.ServiceProvider.GetRequiredService<ISupervisorOperatingData>().GetRoomOperatingDataOfDay(roomId, DateTime.Today)).Select(x => (x.Temperature, x.Date));

            //We begin to delete the false measures after the first 10 measures
            if (temperatures.Count() >= 10)
			{
				(double? temp, DateTime time) median = ZaptoMath.Median(temperatures.TakeLast(10));
				if ((median.temp != null) && (Math.Abs((temperature - median.temp.Value)) < (median.temp * MARGIN)))
				{
					isValid = true;
				}

                //We delete the false measures from the first 10 measures
                if (temperatures.Count() == 10)
				{
					this.DeleteFalseMeasure(scope, median.temp, temperatures, roomId);
                }
			}
			else
			{
				isValid = true;
			}
			return isValid;
		}

        private async Task<bool> CheckHumidityValue(IServiceScope scope, float humidity, string roomId)
        {
            bool isValid = false;
            IEnumerable<(double?, DateTime)> humidities = (await scope.ServiceProvider.GetRequiredService<ISupervisorOperatingData>().GetRoomOperatingDataOfDay(roomId, DateTime.Today)).Select(x => (x.Humidity, x.Date));

			//We begin to delete the false measures after the first 10 measures
            if (humidities.Count() >= 10)
            {
                (double? hum, DateTime time) median = ZaptoMath.Median(humidities.TakeLast(10));
                if ((median.hum != null) && (Math.Abs((humidity - median.hum.Value)) < (median.hum * MARGIN)))
                {
                    isValid = true;
                }

                //We delete the false measures from the first 10 measures
                if (humidities.Count() == 10)
                {
                    this.DeleteFalseMeasure(scope, median.hum, humidities, roomId);
                }
            }
            else
            {
                isValid = true;
            }
            return isValid;
        }

		private void DeleteFalseMeasure(IServiceScope scope, double? median, IEnumerable<(double?, DateTime)> values, string roomId)
		{
			List<DateTime> remove = new List<DateTime>();

            foreach ((double? temp, DateTime time) obj in values)
            {
                if ((median != null) && (obj.temp != null) && (Math.Abs((obj.temp.Value - median.Value)) > (median * MARGIN)))
                {
                    remove.Add(obj.time);
                }
            }

            remove.ForEach(async item => await scope.ServiceProvider.GetRequiredService<ISupervisorOperatingData>().DeleteOperationData(item, roomId));
        }
        #endregion
    }
}

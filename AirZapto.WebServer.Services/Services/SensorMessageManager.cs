using AirZapto.Data;
using AirZapto.Data.Services;
using AirZapto.Model;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace AirZapto.WebServer.Services
{
    public class SensorMessageManager : WebSocketHandler, IWSMessageManager
    {
		#region Properties
		private ISupervisorCacheSensor SupervisorSensor { get;}
		private ISupervisorSensorData SupervisorSensorData { get;}
        #endregion

        #region Constructor
        public SensorMessageManager(WebSockerService webSocketConnectionManager, IServiceProvider serviceProvider, IConfiguration configuration) : base(webSocketConnectionManager)
        {
            this.SupervisorSensor = serviceProvider.GetRequiredService<ISupervisorCacheSensor>();
			this.SupervisorSensorData = serviceProvider.GetRequiredService<ISupervisorSensorData>();		
		}

		#endregion

		#region Methods
        public override async Task<bool> OnDisconnected(WebSocket socket)
        {
			if (socket != null)
			{
				string idSocket = this.WebSocketConnectionManager.GetId(socket);
				if (string.IsNullOrEmpty(idSocket) == false) 
				{
                    await this.SupervisorSensor.DeleteSensorAsync(idSocket);
                }
            }

            return await base.OnDisconnected(socket);
        }

        public override async Task HandleErrorAsync(WebSocket socket)
		{
			if (socket != null)
			{
				string idSocket = this.WebSocketConnectionManager.GetId(socket);
				if (string.IsNullOrEmpty(idSocket) == false)
				{
					(ResultCode code, Sensor? sensor) = await this.SupervisorSensor.GetSensorFromIdSocketAsync(idSocket);
					if ((code == ResultCode.Ok) && (sensor != null))
                    {
						if (socket.State == WebSocketState.Open)
						{
							sensor.Mode = SensorMode.Initial;
							code = await this.SupervisorSensor.UpdateSensorAsync(sensor);
						}
						else
						{
							await this.SupervisorSensor.DeleteSensorAsync(idSocket);
						}
					}
				}
            }
		}
			 
		public override async Task<bool> ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
			bool res = true;

            try
			{
				string received = Encoding.ASCII.GetString(buffer, 0, result.Count).TrimEnd('\0');			
				if (!string.IsNullOrEmpty(received))
				{
					Log.Information(received);

					MessageArduino message = MessageArduino.Deserialize(received);
					if (message != null)
					{
						if ((message.Header == SensorMode.Measure) || (message.Header == SensorMode.Startup))
                        {                            
							SensorConfiguration? configuration = JsonSerializer.Deserialize<SensorConfiguration>(message.Payload);
                            if (configuration != null)
                            {
                                Sensor sensor = new Sensor()
                                {
                                    Channel = configuration.Channel.ToString(),
                                    IdSocket = this.WebSocketConnectionManager.GetId(socket),
                                    Mode = message.Header,
                                    Description = configuration.Description,
                                    Name = configuration.Name,
                                    Period = configuration.Period,
                                    IsRunning = RunningStatus.Healthy,
                                };

                                ResultCode code = await this.SupervisorSensor.AddUpdateSensorAsync(sensor);
                                res = (code == ResultCode.Ok) ? true : false;
                            }
                        }
                        else if (message.Header == SensorMode.Data)
						{
							SensorData? data = JsonSerializer.Deserialize<SensorData>(message.Payload);
							string idSocket = this.WebSocketConnectionManager.GetId(socket);
							(ResultCode code, Sensor? sensor) = await this.SupervisorSensor.GetSensorFromIdSocketAsync(idSocket);
							if ((code == ResultCode.Ok) && (sensor != null) && (data != null))
							{
								sensor.CO2 = data.CO2;
								sensor.Temperature = data.Temperature;
								sensor.Date = Clock.Now;
								sensor.IsRunning = RunningStatus.Healthy;

                                code = await this.SupervisorSensor.UpdateSensorAsync(sensor);
								code = await this.SupervisorSensorData.AddSensorDataAsync(new Model.AirZaptoData()
								{
									CO2 = data.CO2,
									Temperature = data.Temperature,
									SensorId = sensor.Id,
								});
							}
                            res = (code == ResultCode.Ok) ? true : false;
                        }
                        else if (message.Header == SensorMode.Calibration)
						{
							string idSocket = this.WebSocketConnectionManager.GetId(socket);
							(ResultCode code, Sensor? sensor) = await this.SupervisorSensor.GetSensorFromIdSocketAsync(idSocket);
							if ((code == ResultCode.Ok) && (sensor != null))
							{
								sensor.Date = Clock.Now;
								sensor.Mode = message.Header;
                                sensor.IsRunning = RunningStatus.Healthy;
                                code = await this.SupervisorSensor.UpdateSensorAsync(sensor);
							}
                            res = (code == ResultCode.Ok) ? true : false;
                        }
                    }
				}

				return res;
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message);

				if (socket != null)
				{
					string idSocket = this.WebSocketConnectionManager.GetId(socket);
					if (string.IsNullOrEmpty(idSocket) == false)
					{
                        (ResultCode code, Sensor? sensor) = await this.SupervisorSensor.GetSensorFromIdSocketAsync(idSocket);
                        if (code == ResultCode.Ok)
                        {
                            if ((socket.State == WebSocketState.Open) && (sensor != null))
                            {
                                sensor.Mode = SensorMode.Initial;
                                code = await this.SupervisorSensor.UpdateSensorAsync(sensor);
                            }
                            else
                            {
                                await this.SupervisorSensor.DeleteSensorAsync(idSocket);
                            }
                        }
                    }
				}

				return false;
			}
		}

		#endregion
	}
}

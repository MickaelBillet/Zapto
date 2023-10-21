using Connect.Application.Infrastructure;
using Connect.Model;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    internal class ApplicationSensorServices : IApplicationSensorServices
    {
        #region Services
        private ISensorService? SensorService { get; }
        private ISignalRConnectService? SignalRConnectService { get; }
		private IUdpCommunicationService? UdpCommunicationService { get; }
        private IAlertService? AlertService { get; }
        #endregion

        #region Constructor

        public ApplicationSensorServices(IServiceProvider serviceProvider)
		{
            this.SignalRConnectService = serviceProvider.GetService<ISignalRConnectService>();
            this.UdpCommunicationService = serviceProvider.GetService<IUdpCommunicationService>();
            this.SensorService = serviceProvider.GetService<ISensorService>();
            this.AlertService = AlertServiceFactory.CreateAlerteService(serviceProvider, ServiceAlertType.Firebase);
        }

        #endregion

        #region Methods
        /// <summary>
        /// Notify the sensors that they must communicate with the WebServer
        /// </summary>
        /// <returns></returns>
        public async Task<int?> Notify(Sensor sensor)
        {
            int? res = 0;

            try
            {
                (string? data, int port) = sensor.GetSensorConfiguration();
                if (this.UdpCommunicationService != null)
                {
                    res = await this.UdpCommunicationService.SendMessage(data, port, sensor.IpAddress);
                    if (res > 0)
                    {
                        Log.Warning("Notify Sensor : " + sensor.IpAddress + " " + ConnectConstants.PortConnectionData);
                    }
                    else
                    {
                        Log.Error("Error Notification : " + sensor.IpAddress + " " + ConnectConstants.PortConnectionData);
                    }
                }
            }
            catch(Exception ex)
			{
                Log.Fatal(ex.Message);
            }

            return res;
        }

        public async Task<SensorData?> ReceiveDataAsync()
        {
            SensorData? result = null;

            try
            {
                if (this.UdpCommunicationService != null)
                {
                    byte[] data = await this.UdpCommunicationService.StartReception(ConnectConstants.PortSensorData);

                    if (data!.Length > 0)
                    {
                        result = JsonSerializer.Deserialize<SensorData>(Encoding.UTF8.GetString(data, 0, data.Length));
                        Log.Warning("ReceiveDataAsync - SensorData : " + result?.Type + "/" + result?.Channel + " Temperature : " + result?.Temperature + " Humidity : " + result?.Humidity);
                    }
                    else
                    {
                        Log.Error("ReceiveDataAsync - No data");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return result;
        }

        public async Task<SensorEvent?> ReceiveEventAsync()
        {
            SensorEvent? result = null;

            try
            {
                if (this.UdpCommunicationService != null)
                {
                    byte[] data = await this.UdpCommunicationService.StartReception(ConnectConstants.PortSensorEvent);
                    if (data!.Length > 0)
                    {
                        result = JsonSerializer.Deserialize<SensorEvent>(Encoding.UTF8.GetString(data, 0, data.Length));
                        Log.Warning("ReceiveEventAsync - SensorEvent : " + result?.Type + "/" + result?.Channel + " Leak : " + result?.Leak);
                    }
                    else
                    {
                        Log.Error("ReceiveEventAsync - No data");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return result;
        }

        public async Task SendEventToClientAsync(string locationId, Sensor sensor)
        {
            if (this.SignalRConnectService != null)
            {
                await this.SignalRConnectService.SendSensorStatusAsync(locationId, sensor);
                Log.Warning("SendEventToClientAsync : " + sensor.Name + "/" + sensor.Channel + " LeakDetected");
            }
        }

        public async Task<bool?> Leak(string? sensorId, int leakStatus)
        {
            return (this.SensorService != null) ? await this.SensorService.Leak(sensorId, leakStatus.ToString()) : null;
        }

        public async Task NotifySensorLeak(string locationId, Room room)
        {
            if (this.AlertService != null) 
            {
                await this.AlertService.SendAlertAsync(locationId, "Fuite", "Fuite détectée dans la " + room.Name);
            }
        }

        #endregion
    }
}

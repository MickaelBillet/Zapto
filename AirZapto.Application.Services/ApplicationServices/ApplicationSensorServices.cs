using AirZapto.Application.Infrastructure;
using AirZapto.Model;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;


namespace AirZapto.Application.Services
{
    internal class ApplicationSensorServices : IApplicationSensorServices
	{
		#region Services

		private ISensorService? SensorService { get; }

		private IWSMessageManager? NotificationsMessageHandler { get; }

		#endregion

		#region Constructor

		public ApplicationSensorServices(IServiceProvider serviceProvider)
		{
			this.SensorService = serviceProvider.GetService<ISensorService>();
			this.NotificationsMessageHandler = serviceProvider.GetService<IWSMessageManager>();
		}

		#endregion

		#region Methods

		public async Task<IEnumerable<Sensor>?> GetSensorsAsync()
		{
			return (this.SensorService != null) ? await this.SensorService.GetSensors() : null;
		}

		public async Task<Sensor?> GetSensorsAsync(string sensorId)
		{
			return (this.SensorService != null) ? await this.SensorService.GetSensor(sensorId) : null;
		}

		public async Task<bool?> RestartAsync(Sensor sensor)
		{
			return (this.SensorService != null) ? await this.SensorService.RestartSensor(sensor) : null;
		}

		public async Task<bool?> CalibrationAsync(Sensor sensor)
		{
			return (this.SensorService != null) ? await this.SensorService.CalibrationSensor(sensor) : null;
		}

		public async Task<bool> SendCommandAsync(Sensor sensor, int cmd)
		{
			bool isConnected = false;

			if (sensor.IdSocket != null)
			{
				string json = JsonSerializer.Serialize<SensorCommand>(new SensorCommand()
				{
					Command = cmd,
					Channel = sensor.Channel,
					Name = sensor.Name,
					Period = sensor.Period,
				});

                isConnected = (this.NotificationsMessageHandler != null) ? await this.NotificationsMessageHandler.SendMessageAsync(sensor.IdSocket, json) : false;
			}

			return isConnected;
		}

		#endregion
	}
}

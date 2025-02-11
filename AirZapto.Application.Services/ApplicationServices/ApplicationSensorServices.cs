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
		private IWSMessageManager? WSMessageManager { get; }
		#endregion

		#region Constructor
		public ApplicationSensorServices(IServiceProvider serviceProvider)
		{
			this.SensorService = serviceProvider.GetRequiredService<ISensorService>();
			this.WSMessageManager = serviceProvider.GetRequiredService<IWSMessageManager>();
		}
		#endregion

		#region Methods

		public async Task<IEnumerable<Sensor>?> GetSensors()
		{
			return (this.SensorService != null) ? await this.SensorService.GetSensors() : null;
		}

		public async Task<Sensor?> GetSensors(string idSocket)
		{
			return (this.SensorService != null) ? await this.SensorService.GetSensor(idSocket) : null;
		}

		public async Task<bool?> Restart(Sensor sensor)
		{
			return (this.SensorService != null) ? await this.SensorService.RestartSensor(sensor) : null;
		}

		public async Task<bool?> Calibration(Sensor sensor)
		{
			return (this.SensorService != null) ? await this.SensorService.CalibrationSensor(sensor) : null;
		}

		public async Task<bool> SendCommand(Sensor sensor, int cmd)
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

                isConnected = (this.WSMessageManager != null) ? await this.WSMessageManager.SendMessageAsync(sensor.IdSocket, json) : false;
			}

			return isConnected;
		}

		#endregion
	}
}

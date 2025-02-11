using AirZapto.Application.Infrastructure;
using AirZapto.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


namespace AirZapto.Application.Services
{
    internal class ApplicationSensorDataServices : IApplicationSensorDataServices
	{
		#region Services

		private ISensorDataService? SensorDataService { get; }

		#endregion

		#region Constructor

		public ApplicationSensorDataServices(IServiceProvider serviceProvider)
		{
			this.SensorDataService = serviceProvider.GetRequiredService<ISensorDataService>();
		}

		#endregion

		#region Methods

		public async Task<IEnumerable<AirZaptoData>?> GetSensorData(string? sensorId, int duration)
		{
			try
			{
				return  (this.SensorDataService != null) ? await this.SensorDataService.GetSensorData(sensorId, duration) : null;
			}
			catch (HttpRequestException ex) when (ex.Message.Contains(HttpStatusCode.NotFound.ToString()))
			{
				return null;
			}
		}

		#endregion
	}
}

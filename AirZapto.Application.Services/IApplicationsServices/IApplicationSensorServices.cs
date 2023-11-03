using AirZapto.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirZapto.Application
{
	public interface IApplicationSensorServices
	{
		public Task<bool?> RestartAsync(Sensor sensor);
		public Task<bool?> CalibrationAsync(Sensor sensor);
		public Task<IEnumerable<Sensor>?> GetSensorsAsync();
		public Task<bool> SendCommandAsync(Sensor sensor, int command);
		public Task<Sensor?> GetSensorsAsync(string sensorId);
	}
}

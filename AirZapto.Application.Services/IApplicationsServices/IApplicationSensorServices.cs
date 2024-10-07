using AirZapto.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirZapto.Application
{
	public interface IApplicationSensorServices
	{
		public Task<bool?> Restart(Sensor sensor);
		public Task<bool?> Calibration(Sensor sensor);
		public Task<IEnumerable<Sensor>?> GetSensors();
		public Task<bool> SendCommand(Sensor sensor, int command);
		public Task<Sensor?> GetSensors(string sensorId);
	}
}

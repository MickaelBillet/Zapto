using AirZapto.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirZapto.Application.Infrastructure
{
	public interface ISensorService
	{
		public Task<IEnumerable<Sensor>?> GetSensors();
		public Task<bool?> CalibrationSensor(Sensor sensor);
		public Task<bool?> RestartSensor(Sensor sensor);
		Task<Sensor?> GetSensor(string idSocket);
	}
}

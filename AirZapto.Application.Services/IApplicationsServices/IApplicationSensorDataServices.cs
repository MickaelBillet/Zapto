using AirZapto.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirZapto.Application
{
	public interface IApplicationSensorDataServices
	{
		public Task<IEnumerable<AirZaptoData>?> GetSensorData(string sensorId, int duration);
	}
}

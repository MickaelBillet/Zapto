using AirZapto.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirZapto.Application
{
	public interface IApplicationSensorDataServices
	{
		public Task<IEnumerable<AirZaptoData>?> GetSensorDataAsync(string sensorId, int duration);
	}
}

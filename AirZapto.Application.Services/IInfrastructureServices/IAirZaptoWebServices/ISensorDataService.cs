using System.Collections.Generic;
using System.Threading.Tasks;
using AirZapto.Model;

namespace AirZapto.Application.Infrastructure
{
	public interface ISensorDataService
	{
		public Task<IEnumerable<AirZaptoData>?> GetSensorData(string? sensorId, int duration);
	}
}

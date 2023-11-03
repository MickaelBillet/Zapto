using AirZapto.Model;
using Framework.Core.Base;

namespace AirZapto.Data
{
    public interface ISupervisorSensorData
	{
		#region SensorData
		Task<ResultCode> DeleteSensorDataAsync(TimeSpan span);
		Task<ResultCode> AddSensorDataAsync(AirZaptoData data);
		Task<(ResultCode, IEnumerable<AirZaptoData>?)> GetSensorDataAsync(string sensorId, int minutes);
		Task<DateTime?> GetTimeLastSensorDataAsync(string sensorId);
        #endregion
    }
}

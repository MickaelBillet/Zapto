using AirZapto.Model;
using Framework.Core.Base;

namespace AirZapto.Data
{
    public interface ISupervisorSensor
	{
		#region Sensor
		Task<ResultCode> UpdateSensorAsync(Sensor sensor);
		Task<(ResultCode, Sensor?)> GetSensorAsync(string id);
		Task<(ResultCode, IEnumerable<Sensor>?)> GetSensorsAsync();
		Task<(ResultCode, IEnumerable<Sensor>?)> GetCacheSensorsAsync();
        Task<(ResultCode, Sensor?)> GetSensorFromIdSocketAsync(string idSocket);
		Task<ResultCode> AddUpdateSensorAsync(Sensor sensor);
		Task<ResultCode> DeleteSensorAsync(string idSocket);
		#endregion
    }
}

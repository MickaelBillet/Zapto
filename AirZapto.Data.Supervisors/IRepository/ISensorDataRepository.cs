using AirZapto.Data.Entities;

namespace AirZapto.Data.Services.Repositories
{
    public interface ISensorDataRepository : IDisposable
	{
		#region SensorData
		Task<IEnumerable<SensorDataEntity>?> GetSensorDataAsync(string sensorId, int minutes);
		Task<bool> AddSensorDataAsync(SensorDataEntity entity);
		Task<bool> DeleteSensorDataAsync(TimeSpan span);
		Task<DateTime?> GetTimeLastSensorData(string sensorId);
        #endregion
    }
}

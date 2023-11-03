using AirZapto.Data.Entities;

namespace AirZapto.Data.Services.Repositories
{
    public interface IRepository : IDisposable
	{
		#region Sensor
		Task<IEnumerable<SensorEntity>?> GetAllSensorsAsync();
		Task<bool> UpdateSensorAsync(SensorEntity entity);
		Task<SensorEntity?> GetSensorAsync(string id);
		Task<bool> AddSensorAsync(SensorEntity entity);
		Task<SensorEntity?> GetSensorFromIdSocketAsync(string idSocket);
		Task<bool> DeleteSensorAsync(SensorEntity entity);
		Task<SensorEntity?> GetSensorAsync(string channel, string? name);
		#endregion

		#region SensorData
		Task<IEnumerable<SensorDataEntity>?> GetSensorDataAsync(string sensorId, int minutes);
		Task<bool> AddSensorDataAsync(SensorDataEntity entity);
		Task<bool> DeleteSensorDataAsync(TimeSpan span);
		Task<DateTime?> GetTimeLastSensorData(string sensorId);
        #endregion

        #region Logs
        LogsEntity? GetLogs(string id);
		Task<IEnumerable<LogsEntity>?> GetAllLogsAsync();
		bool LogsExists(string sensorId);
		bool AddLogs(LogsEntity entity);
		Task<bool> DeleteLogsAsync(LogsEntity entity);
		Task<bool> UpdateLogsAsync(LogsEntity entity);
		#endregion

		#region Version
		Task<VersionEntity?> GetVersionAsync();
		Task<bool> AddVersionAsync(VersionEntity entity);
		Task<bool> UpdateVersionAsync(VersionEntity entity);
        #endregion
    }
}

using AirZapto.Data.Entities;

namespace AirZapto.Data.Services.Repositories
{
    public interface ISensorRepository : IDisposable
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
    }
}

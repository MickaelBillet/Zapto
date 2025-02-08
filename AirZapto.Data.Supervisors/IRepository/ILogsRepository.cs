using AirZapto.Data.Entities;

namespace AirZapto.Data.Services.Repositories
{
    public interface ILogsRepository : IDisposable
	{
        #region Logs
        LogsEntity? GetLogs(string id);
		Task<IEnumerable<LogsEntity>?> GetAllLogsAsync();
		bool LogsExists(string sensorId);
		bool AddLogs(LogsEntity entity);
		Task<bool> DeleteLogsAsync(LogsEntity entity);
		Task<bool> UpdateLogsAsync(LogsEntity entity);
		#endregion
    }
}

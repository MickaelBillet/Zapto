using Framework.Core.Base;
using Framework.Core.Domain;

namespace AirZapto.Data
{
    public interface ISupervisorLogs
	{
        #region Logs
        ResultCode AddLogs(Logs logs);
		Task<(ResultCode, IEnumerable<Logs>?)> GetLogsAsync();
		ResultCode LogsExist(string id);
		Task<(ResultCode, IEnumerable<Logs>?)> GetLogsInf24HAsync();
        #endregion
    }
}

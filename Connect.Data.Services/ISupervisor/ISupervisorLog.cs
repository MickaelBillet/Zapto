using Framework.Core.Base;
using Framework.Core.Domain;

namespace Connect.Data
{
    public interface ISupervisorLog
    {
        Task<IEnumerable<Logs>> GetLogsInf24H();
        Task<IEnumerable<Logs>> GetLogsCollection();
        Task<ResultCode> AddLog(Logs log);
        Task<ResultCode> LogExists(string id);
        Task<Logs> GetLogs(string id);
        Task<ResultCode> DeleteLogs(Logs log);
    }
}

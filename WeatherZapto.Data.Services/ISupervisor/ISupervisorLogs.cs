using Framework.Core.Base;
using Framework.Core.Domain;

namespace WeatherZapto.Data
{
    public interface ISupervisorLogs
    {
        Task<IEnumerable<Logs>> GetLogsInf24H();
        Task<IEnumerable<Logs>> GetLogsCollection();
        Task<ResultCode> AddLog(Logs log);
    }
}

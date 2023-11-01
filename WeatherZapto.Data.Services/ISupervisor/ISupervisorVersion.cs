using Framework.Core.Base;

namespace WeatherZapto.Data
{
    public interface ISupervisorVersion
    {
        Task<Version> GetVersion();
        Task<ResultCode> AddVersion();
        Task<ResultCode> UpdateVersion(int major, int minor, int build);
    }
}

using Framework.Core.Base;

namespace WeatherZapto.Data
{
    public interface ISupervisorCall
    {
        Task<ResultCode> AddCallOW();
    }
}

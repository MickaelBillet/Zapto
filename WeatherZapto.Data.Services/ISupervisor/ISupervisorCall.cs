using Framework.Core.Base;

namespace WeatherZapto.Data
{
    public interface ISupervisorCall
    {
        Task<ResultCode> AddCallOpenWeather();
        Task<long?> GetDayCallsCount(DateOnly date);
        Task<long?> GetLast30DaysCallsCount();
    }
}

using Framework.Core.Base;

namespace WeatherZapto.Data
{
    public interface ISupervisorCall
    {
        Task<ResultCode> AddCallOpenWeather();
        Task<long?> GetDayCallsCount(DateTime date);
        Task<long?> GetLast30DaysCallsCount();
    }
}

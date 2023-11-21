namespace WeatherZapto.Application
{
    public interface IApplicationCallOWService
    {
        Task<int> GetCurrentDayCallsCount();
        Task<long> GetLast30DaysCallsCount();
    }
}

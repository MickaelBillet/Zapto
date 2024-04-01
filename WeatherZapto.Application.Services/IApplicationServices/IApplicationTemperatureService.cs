namespace WeatherZapto.Application.Services
{
    public interface IApplicationTemperatureService
    {
        Task<IEnumerable<double?>> GetTemperatureOfDay(string location, DateTime? dateTime, CancellationToken token = default);
    }
}

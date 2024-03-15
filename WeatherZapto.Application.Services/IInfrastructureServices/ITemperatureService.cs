namespace WeatherZapto.Application.Infrastructure
{
    public interface ITemperatureService
    {
        Task<IEnumerable<double?>> GetHomeTemperatures(DateTime? day, string location, CancellationToken cancellationToken);
    }
}

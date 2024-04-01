namespace WeatherZapto.Application.Infrastructure
{
    public interface ITemperatureService
    {
        Task<IEnumerable<double?>> GetLocationTemperatures(DateTime dateTime, string location, CancellationToken cancellationToken);
    }
}

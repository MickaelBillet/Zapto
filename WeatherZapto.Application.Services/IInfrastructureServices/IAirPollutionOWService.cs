using WeatherZapto.Model;

namespace WeatherZapto.Application.Infrastructure
{
    public interface IAirPollutionOWService
    {
        Task<AirPollution> GetAirPollution(string APIKey, string longitude, string latitude);
    }
}

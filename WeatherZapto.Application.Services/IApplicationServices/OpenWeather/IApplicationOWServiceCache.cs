using WeatherZapto.Model;

namespace WeatherZapto.Application
{
    public interface IApplicationOWServiceCache
    {
        Task<ZaptoAirPollution> GetCurrentAirPollution(string locationName, string longitude, string latitude);
        Task<ZaptoWeather> GetCurrentWeather(string locationName, string longitude, string latitude, string language);
    }
}

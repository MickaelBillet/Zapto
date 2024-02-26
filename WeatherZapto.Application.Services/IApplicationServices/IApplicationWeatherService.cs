using WeatherZapto.Model;

namespace WeatherZapto.Application
{
    public interface IApplicationWeatherService
    {
        Task<ZaptoWeather> GetCurrentWeather(string locationName, string longitude, string latitude, string culture);
        Task<ZaptoWeather> GetCurrentWeather(string longitude, string latitude, string culture);
    }
}

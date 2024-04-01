using WeatherZapto.Model;

namespace WeatherZapto.Application.Infrastructure
{
    public interface IWeatherService
    {
        Task<ZaptoWeather> GetWeather(string longitude, string latitude, string culture);
        Task<ZaptoWeather> GetWeather(string location, string longitude, string latitude, string culture);
    }
}

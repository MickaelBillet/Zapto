using WeatherZapto.Model;

namespace WeatherZapto.Application
{
    public interface IApplicationOWService
    {
        Task<ZaptoAirPollution> GetCurrentAirPollution(string APIKey, string locationName, string longitude, string latitude);
        Task<ZaptoLocation> GetReverseLocation(string APIKey, string longitude, string latitude);
        Task<ZaptoWeather> GetCurrentWeather(string APIKey, string locationName, string longitude, string latitude, string language);
        Task<Stream> GetCurrentWeatherImage(string code);
        Task<IEnumerable<ZaptoLocation>> GetLocationFromCity(string APIKey, string city, string stateCode, string countryCode);
        Task<ZaptoLocation> GetLocationFromZipCode(string APIKey, string zipCode, string countryCode);
    }
}

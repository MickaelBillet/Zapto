using WeatherZapto.Model;

namespace WeatherZapto.Application
{
    public interface IApplicationOWService
    {
        Task<ZaptoAirPollution> GetCurrentAirPollution(string locationName, string longitude, string latitude);
        Task<ZaptoLocation> GetReverseLocation(string longitude, string latitude);
        Task<ZaptoWeather> GetCurrentWeather(string locationName, string longitude, string latitude, string language);
        Task<Stream> GetCurrentWeatherImage(string code);
        Task<IEnumerable<ZaptoLocation>> GetLocationFromCity(string city, string stateCode, string countryCode);
        Task<ZaptoLocation> GetLocationFromZipCode(string zipCode, string countryCode);
    }
}

using WeatherZapto.Model;

namespace WeatherZapto.Application.Infrastructure
{
    public interface ILocationOWService
    {
        Task<IEnumerable<Location>> GetReverseLocations(string APIKey, string longitude, string latitude);
        Task<IEnumerable<Location>> GetLocationsFromCity(string APIKey, string city, string stateCode, string countryCode);
        Task<Location> GetLocationFromZipCode(string APIKey, string zipCode, string countryCode);
    }
}

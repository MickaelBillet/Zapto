using WeatherZapto.Model;

namespace WeatherZapto.Application.Infrastructure
{
    public interface ILocationService
    {
        Task<ZaptoLocation> GetReverseLocation(string longitude, string latitude);
        Task<IEnumerable<ZaptoLocation>> GetLocations(string city, string stateCode, string countryCode);
        Task<ZaptoLocation> GetLocation(string zipCode, string countryCode);
    }
}

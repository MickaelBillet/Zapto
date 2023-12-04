using WeatherZapto.Model;

namespace WeatherZapto.Application
{
    public interface IApplicationLocationService
    {
        Task<ZaptoLocation> GeReversetLocation(string longitude, string latitude);
        Task<IEnumerable<ZaptoLocation>> GetLocations(string city, string stateCode, string countryCode);
        Task<ZaptoLocation> GetLocation(string zipCode, string countryCode);
    }
}

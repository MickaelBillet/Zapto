using System.Threading.Tasks;
using WeatherZapto.Model;

namespace WeatherZapto.Application
{
    public interface IApplicationOWServiceCache
    {
        Task<ZaptoAirPollution> GetCurrentAirPollution(string APIKey, string locationName, string longitude, string latitude);
        Task<ZaptoWeather> GetCurrentWeather(string APIKey, string locationName, string longitude, string latitude, string language);
    }
}

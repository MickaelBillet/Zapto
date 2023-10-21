using System.Threading.Tasks;
using WeatherZapto.Model;

namespace WeatherZapto.Application
{
    public interface IApplicationAirPollutionService
    {
        Task<ZaptoAirPollution> GetCurrentAirPollution(string locationName, string longitude, string latitude);
        Task<ZaptoAirPollution> GetCurrentAirPollution(string longitude, string latitude);
    }
}

using System.Threading.Tasks;
using WeatherZapto.Model;

namespace WeatherZapto.Application.Infrastructure
{
    public interface IAirPollutionService
    {
        Task<ZaptoAirPollution> GetAirPollution(string longitude, string latitude);
        Task<ZaptoAirPollution> GetAirPollution(string location, string longitude, string latitude);
    }
}

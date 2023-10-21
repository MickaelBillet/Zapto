using System.Threading.Tasks;
using WeatherZapto.Model;

namespace WeatherZapto.Application.Infrastructure
{
    public interface ILocationService
    {
        Task<ZaptoLocation> GetLocation(string longitude, string latitude);
    }
}

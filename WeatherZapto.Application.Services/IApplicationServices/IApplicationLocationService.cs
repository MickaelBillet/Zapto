using System.Threading.Tasks;
using WeatherZapto.Model;

namespace WeatherZapto.Application
{
    public interface IApplicationLocationService
    {
        Task<ZaptoLocation> GetLocation(string longitude, string latitude);
    }
}

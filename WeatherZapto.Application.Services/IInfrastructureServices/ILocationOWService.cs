using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherZapto.Model;

namespace WeatherZapto.Application.Infrastructure
{
    public interface ILocationOWService
    {
        Task<IEnumerable<Location>> GetLocations(string APIKey, string longitude, string latitude);
    }
}

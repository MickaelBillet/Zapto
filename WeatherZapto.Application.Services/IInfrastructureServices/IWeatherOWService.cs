using System.IO;
using System.Threading.Tasks;
using WeatherZapto.Model;

namespace WeatherZapto.Application.Infrastructure
{
    public interface IWeatherOWService
    {
        Task<OpenWeather> GetWeather(string APIKey, string longitude, string latitude, string language);
        Task<Stream> GetWeatherImage(string code);
    }
}

using Framework.Core.Base;
using WeatherZapto.Model;

namespace WeatherZapto.Data
{
    public interface ISupervisorWeather
    {
        Task<ResultCode> AddWeatherAsync(ZaptoWeather weather);
        Task<ResultCode> DeleteWeatherAsync(ZaptoWeather weather);
    }
}

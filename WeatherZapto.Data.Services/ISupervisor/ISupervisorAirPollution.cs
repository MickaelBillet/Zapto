using Framework.Core.Base;
using WeatherZapto.Model;

namespace WeatherZapto.Data
{
    public interface ISupervisorAirPollution
    {
        Task<ResultCode> AddAirPollutionAsync(ZaptoAirPollution airPollution);
        Task<ResultCode> DeleteAirPollutionAsync(ZaptoAirPollution airPollution);
    }
}

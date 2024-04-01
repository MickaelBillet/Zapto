using AirZapto.Model;
using Framework.Core.Base;

namespace AirZapto.Data.Services
{
    public interface ISupervisorCacheSensor
    {
        Task<ResultCode> AddUpdateSensorAsync(Sensor sensor);
        Task<ResultCode> UpdateSensorAsync(Sensor sensor);
        Task<(ResultCode, Sensor?)> GetSensorFromIdSocketAsync(string idSocket);
        Task<ResultCode> DeleteSensorAsync(string idSocket);
        Task<(ResultCode, IEnumerable<Sensor>?)> GetSensorsAsync();
    }
}

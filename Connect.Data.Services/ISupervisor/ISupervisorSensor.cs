using Connect.Model;
using Framework.Core.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.Data
{
    public interface ISupervisorSensor
    {
        Task<ResultCode> SensorExists(string id);
        Task<IEnumerable<Sensor>> GetSensors();
        Task<Sensor> GetSensor(string id);
        Task<IEnumerable<Sensor>> GetSensors(string roomId);
        Task<Sensor> GetSensor(string? type, string? channel);
        Task<ResultCode> AddSensor(Sensor sensor);
        Task<(ResultCode, Sensor)> UpdateSensor(Sensor sensor);
        Task<ResultCode> DeleteSensor(Sensor sensor);
    }
}

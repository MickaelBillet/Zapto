using Connect.Model;
using Framework.Core.Base;

namespace Connect.Data
{
    public interface ISupervisorCacheSensor : ISupervisorCache
    {
        public Task<(ResultCode, Sensor)> UpdateSensor(Sensor sensor);
        public Task<ResultCode> AddSensor(Sensor sensor);
        public Task<Sensor> GetSensor(string id);
        public Task<Sensor> GetSensor(string? type, string? channel);
        public Task<IEnumerable<Sensor>> GetSensors();
    }
}

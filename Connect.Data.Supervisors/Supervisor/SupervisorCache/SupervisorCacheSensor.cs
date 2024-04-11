using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public class SupervisorCacheSensor : SupervisorCache, ISupervisorSensor
    {
        #region Services
        private ISupervisorSensor Supervisor { get; }
        #endregion

        #region Constructor
        public SupervisorCacheSensor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.Supervisor = serviceProvider.GetRequiredService<ISupervisorFactorySensor>().CreateSupervisor(0);
        }
        #endregion

        #region Methods
        public async Task Initialize()
        {
            IEnumerable<Sensor> sensors = await this.Supervisor.GetSensors();
            foreach (var item in sensors)
            {
                await this.CacheSensorService.Set(item.Id, item);
            }
        }
        public async Task<(ResultCode, Sensor)> UpdateSensor(Sensor sensor)
        {
            (ResultCode code, Sensor sensor) obj = await this.Supervisor.UpdateSensor(sensor);
            if (obj.code == ResultCode.Ok)
            {
                await this.CacheSensorService?.Set(sensor.Id, sensor);
            }
            return (obj.code, obj.sensor);
        }
        public async Task<ResultCode> AddSensor(Sensor sensor)
        {
            ResultCode code = await this.Supervisor.AddSensor(sensor);
            if (code == ResultCode.Ok)
            {
                await this.CacheSensorService.Set(sensor.Id, sensor);
            }
            return code;
        }
        public async Task<Sensor> GetSensor(string id)
        {
            Sensor sensor = await this.CacheSensorService.Get((arg) => arg.Id == id);
            return sensor;
        }
        public async Task<Sensor> GetSensor(string type, string channel)
        {
            Sensor sensor = await this.CacheSensorService.Get((arg) => arg.Name == type && arg.Channel == channel);
            return sensor;
        }
        public async Task<IEnumerable<Sensor>> GetSensors()
        {
            IEnumerable<Sensor> sensors = await this.CacheSensorService.GetAll();
            return sensors;
        }

        #endregion
    }
}

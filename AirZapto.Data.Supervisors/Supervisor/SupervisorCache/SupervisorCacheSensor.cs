using AirZapto.Data.Services;
using AirZapto.Model;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AirZapto.Data.Supervisors
{
    public sealed class SupervisorCacheSensor : ISupervisorCacheSensor
    {
        #region Services
        private ISupervisorSensor Supervisor { get; }
        private ICacheZaptoService<Sensor> CacheService { get; }
        #endregion

        #region Constructor
        public SupervisorCacheSensor(IServiceProvider serviceProvider)
        {
            this.Supervisor = serviceProvider.GetRequiredService<ISupervisorSensor>();
            this.CacheService = serviceProvider.GetRequiredService<ICacheZaptoService<Sensor>>();
        }
        #endregion

        #region Methods
        public async Task<ResultCode> AddUpdateSensorAsync(Sensor sensor)
        {
            await this.CacheService.Set(sensor.IdSocket, sensor);
            return await this.Supervisor.AddUpdateSensorAsync(sensor);
        }

        public async Task<ResultCode> UpdateSensorAsync(Sensor sensor)
        {
            await this.CacheService.Set(sensor.IdSocket, sensor);
            return await this.Supervisor.UpdateSensorAsync(sensor);
        }

        public async Task<(ResultCode, Sensor?)> GetSensorFromIdSocketAsync(string idSocket)
        {
            ResultCode code = ResultCode.Ok;
            Sensor? sensor = await this.CacheService.Get((sensor) => sensor.IdSocket == idSocket);
            if (sensor == null)
            {
                (code, sensor) = await this.Supervisor.GetSensorFromIdSocketAsync(idSocket);
                if ((code  == ResultCode.Ok)  && (sensor != null))
                {
                    await this.CacheService.Set(sensor.IdSocket, sensor);
                }
            }
            return (code, sensor);
        }

        public async Task<ResultCode> DeleteSensorAsync(string idSocket)
        {
            await this.CacheService.Delete(idSocket);
            return await this.Supervisor.DeleteSensorAsync(idSocket);
        }

        public async Task<(ResultCode, IEnumerable<Sensor>?)> GetSensorsAsync()
        {
            return (ResultCode.Ok, await this.CacheService.GetAll());
        }
        #endregion
    }
}

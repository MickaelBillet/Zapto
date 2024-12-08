using AirZapto.Data.Services.Repositories;
using AirZapto.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using Framework.Infrastructure.Services;

namespace AirZapto.Data.Supervisors
{
    public sealed class SupervisorCacheSensor : ISupervisorSensor
    {
        #region Services
        private ISupervisorSensor Supervisor { get; }
        private ICacheZaptoService<Sensor> CacheService { get; }
        #endregion

        #region Constructor
        public SupervisorCacheSensor(IDalSession session, IDataContextFactory contextFactory, IRepositoryFactory repositoryFactory, ICacheZaptoService<Sensor> cacheZapto)
        {
            this.Supervisor = new SupervisorSensor(session, contextFactory, repositoryFactory);
            this.CacheService = cacheZapto;
        }
        #endregion

        #region Methods
        public async Task<ResultCode> AddUpdateSensorAsync(Sensor sensor)
        {
            ResultCode code = await this.Supervisor.AddUpdateSensorAsync(sensor);
            if (code == ResultCode.Ok)
            {
                await this.CacheService.Set(sensor.IdSocket, sensor);
            }
            return code;
        }

        public async Task<ResultCode> UpdateSensorAsync(Sensor sensor)
        {
            ResultCode code = await this.Supervisor.UpdateSensorAsync(sensor);
            if (code == ResultCode.Ok)
            {
                await this.CacheService.Set(sensor.IdSocket, sensor);
            }
            return code; 
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
            ResultCode code = await this.Supervisor.DeleteSensorAsync(idSocket);
            if (code == ResultCode.Ok)
            {
                await this.CacheService.Delete(idSocket);
            }
            return code;
        }

        public async Task<(ResultCode, IEnumerable<Sensor>?)> GetSensorsAsync()
        {
            return (ResultCode.Ok, await this.CacheService.GetAll());
        }
        #endregion
    }
}

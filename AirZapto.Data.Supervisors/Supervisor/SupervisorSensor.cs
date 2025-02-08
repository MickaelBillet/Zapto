using AirZapto.Data.Entities;
using AirZapto.Data.Mappers;
using AirZapto.Data.Services.Repositories;
using AirZapto.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;

namespace AirZapto.Data.Supervisors
{
    public sealed class SupervisorSensor : Supervisor, ISupervisorSensor
	{
        private readonly Lazy<ISensorRepository>? _lazySensorRepository;

        #region Properties
        private ISensorRepository SensorDataRepository => _lazySensorRepository!.Value;
        #endregion

        #region Constructor
        public SupervisorSensor(IDalSession session, IRepositoryFactory repositoryFactory) : base()
        {
            _lazySensorRepository = repositoryFactory.CreateSensorRepository(session);
        }
        #endregion

        #region Methods
        public async Task<ResultCode> UpdateSensorAsync(Sensor sensor)
		{
			ResultCode result = ResultCode.CouldNotUpdateItem;
			if (this.SensorDataRepository != null)
			{
				SensorEntity entity = SensorMapper.Map(sensor);
				result = (await this.SensorDataRepository.UpdateSensorAsync(entity) == true) ? ResultCode.Ok : ResultCode.CouldNotUpdateItem;
			}
			return result;
		}

		public async Task<ResultCode> AddUpdateSensorAsync(Sensor sensor)
		{
			ResultCode result = ResultCode.CouldNotCreateItem;
			if (this.SensorDataRepository != null)
			{
				if (sensor.IdSocket != null)
				{ 
					SensorEntity? entity = await this.SensorDataRepository.GetSensorAsync(sensor.Channel, sensor.Name);
					if (entity != null)
					{
						sensor.Id = entity.Id;
                        entity = SensorMapper.Map(sensor);
                        result = (await this.SensorDataRepository.UpdateSensorAsync(entity) == true) ? ResultCode.Ok : ResultCode.CouldNotUpdateItem;
					}
					else
					{
						sensor.Id = string.IsNullOrEmpty(sensor.Id) ? Guid.NewGuid().ToString() : sensor.Id;
						sensor.Date = Clock.Now;
                        entity = SensorMapper.Map(sensor);
						result = (await this.SensorDataRepository.AddSensorAsync(entity) == true) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
					}
				}
			}

			return result;
		}

		public async Task<(ResultCode, Sensor?)> GetSensorFromIdSocketAsync(string idSocket)
		{
			ResultCode result = ResultCode.ItemNotFound;
			Sensor? sensor = null;
			if (this.SensorDataRepository != null)
			{
				SensorEntity? entity = await this.SensorDataRepository.GetSensorFromIdSocketAsync(idSocket);
				sensor = (entity != null) ? SensorMapper.Map(entity) : null;
				result = (sensor != null) ? ResultCode.Ok : ResultCode.ItemNotFound;
			}

			return (result, sensor);
		}

		public async Task<ResultCode> DeleteSensorAsync(string idSocket)
		{
			ResultCode result = ResultCode.CouldNotDeleteItem;
			if (this.SensorDataRepository != null)
			{
				SensorEntity? entity = await this.SensorDataRepository.GetSensorFromIdSocketAsync(idSocket);
				if (entity != null)
				{
					result = (await this.SensorDataRepository.DeleteSensorAsync(entity) == true) ? ResultCode.Ok : ResultCode.CouldNotDeleteItem;
				}
				else
				{
					result = ResultCode.ItemNotFound;
				}
			}

			return result;
		}

        public async Task<(ResultCode, IEnumerable<Sensor>?)> GetSensorsAsync()
        {
            ResultCode result = ResultCode.ItemNotFound;
            IEnumerable<Sensor>? sensors = null;
            if (this.SensorDataRepository != null)
            {
                IEnumerable<SensorEntity>? entities = await this.SensorDataRepository.GetAllSensorsAsync();
                sensors = (entities != null) ? entities.Select((item) => SensorMapper.Map(item)) : null;
                if ((sensors != null) && (sensors.Any()))
                {
                    result = ResultCode.Ok;
                }
                else
                {
                    result = ResultCode.ItemNotFound;
                }
            }

            return (result, sensors);
        }
        #endregion
    }
}

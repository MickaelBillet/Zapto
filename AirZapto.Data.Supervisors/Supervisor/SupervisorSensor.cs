using AirZapto.Data.Entities;
using AirZapto.Data.Mappers;
using AirZapto.Model;
using Framework.Core.Base;

namespace AirZapto.Data.Supervisors
{
    public sealed class SupervisorSensor : Supervisor, ISupervisorSensor
	{
        #region Services

        #endregion

        #region Constructor
        public SupervisorSensor(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
        #endregion

        #region Methods
        public async Task<ResultCode> UpdateSensorAsync(Sensor sensor)
		{
			ResultCode result = ResultCode.CouldNotUpdateItem;
			if (this.Repository != null)
			{
				SensorEntity entity = SensorMapper.Map(sensor);
				result = (await this.Repository.UpdateSensorAsync(entity) == true) ? ResultCode.Ok : ResultCode.CouldNotUpdateItem;
			}
			return result;
		}

		public async Task<ResultCode> AddUpdateSensorAsync(Sensor sensor)
		{
			ResultCode result = ResultCode.CouldNotCreateItem;
			if (this.Repository != null)
			{
				if (sensor.IdSocket != null)
				{ 
					SensorEntity? entity = await this.Repository.GetSensorAsync(sensor.Channel, sensor.Name);
					if (entity != null)
					{
						sensor.Id = entity.Id;
                        entity = SensorMapper.Map(sensor);
                        result = (await this.Repository.UpdateSensorAsync(entity) == true) ? ResultCode.Ok : ResultCode.CouldNotUpdateItem;
					}
					else
					{
						sensor.Id = string.IsNullOrEmpty(sensor.Id) ? Guid.NewGuid().ToString() : sensor.Id;
						sensor.Date = Clock.Now;
                        entity = SensorMapper.Map(sensor);
						result = (await this.Repository.AddSensorAsync(entity) == true) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
					}
				}
			}

			return result;
		}

		public async Task<(ResultCode, Sensor?)> GetSensorAsync(string id)
		{
			ResultCode result = ResultCode.ItemNotFound;
			Sensor? sensor = null;
            if (this.Repository != null)
            {
                SensorEntity? entity = await this.Repository.GetSensorAsync(id);
                sensor = (entity != null) ? SensorMapper.Map(entity) : null;
                if (sensor != null)
                {
                    result = ResultCode.Ok;
                }
                else
                {
                    result = ResultCode.ItemNotFound;
                }
            }

            return (result, sensor);
		}

		public async Task<(ResultCode, Sensor?)> GetSensorFromIdSocketAsync(string idSocket)
		{
			ResultCode result = ResultCode.ItemNotFound;
			Sensor? sensor = null;
			if (this.Repository != null)
			{
				SensorEntity? entity = await this.Repository.GetSensorFromIdSocketAsync(idSocket);
				sensor = (entity != null) ? SensorMapper.Map(entity) : null;
				result = (sensor != null) ? ResultCode.Ok : ResultCode.ItemNotFound;
			}

			return (result, sensor);
		}

		public async Task<ResultCode> DeleteSensorAsync(string idSocket)
		{
			ResultCode result = ResultCode.CouldNotDeleteItem;
			if (this.Repository != null)
			{
				SensorEntity? entity = await this.Repository.GetSensorFromIdSocketAsync(idSocket);

				if (entity != null)
				{
					result = (await this.Repository.DeleteSensorAsync(entity) == true) ? ResultCode.Ok : ResultCode.CouldNotDeleteItem; ;
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
            if (this.Repository != null)
            {
                IEnumerable<SensorEntity>? entities = await this.Repository.GetAllSensorsAsync();
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

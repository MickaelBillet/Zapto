using AirZapto.Data.Entities;
using AirZapto.Data.Mappers;
using AirZapto.Model;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AirZapto.Data.Supervisors
{
    public sealed class SupervisorSensor : Supervisor, ISupervisorSensor
	{
        #region Services
        private IMemoryCache? Cache { get; }
        private CacheSignal? CacheSignal { get; }
        private MemoryCacheEntryOptions MemoryCacheEntryOptions { get; }
        #endregion

        #region Constructor
        public SupervisorSensor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.Cache = serviceProvider.GetService<IMemoryCache>();
            this.CacheSignal = serviceProvider.GetService<CacheSignal>();
            this.MemoryCacheEntryOptions = new MemoryCacheEntryOptions()
                                                       .SetSlidingExpiration(TimeSpan.FromSeconds(600)) //This determines how long a cache entry can be inactive before it is removed from the cache
                                                       .SetAbsoluteExpiration(TimeSpan.FromSeconds(900)) //The problem with sliding expiration is that if we keep on accessing the cache entry, it will never expire
                                                       .SetPriority(CacheItemPriority.Normal);
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
			if (this.CacheSignal != null && this.Cache != null)
			{
				try
				{
                    await this.CacheSignal.WaitAsync();
					if (this.Cache.TryGetValue($"Sensor-{id}", out sensor))
					{
						Log.Information($"Sensor found {sensor}");
					}
					else
					{
						if (this.Repository != null)
						{
							SensorEntity? entity = await this.Repository.GetSensorAsync(id);
							sensor = (entity != null) ? SensorMapper.Map(entity) : null;
							if (sensor != null) 
							{
								this.Cache.Set($"Sensor-{id}", sensor, this.MemoryCacheEntryOptions);
								result = ResultCode.Ok;
                            }
							else
							{
								result = ResultCode.ItemNotFound;
							}
						}
					}
				}
                finally
                {
                    this.CacheSignal.Release();
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
            if (this.CacheSignal != null && this.Cache != null)
            {
				try
				{
					await this.CacheSignal.WaitAsync();
					if (this.Cache.TryGetValue($"Sensor", out sensors))
					{
						Log.Information($"Sensors found");
					}
					else
					{
						if (this.Repository != null)
						{
							IEnumerable<SensorEntity>? entities = await this.Repository.GetAllSensorsAsync();
							sensors = (entities != null) ? entities.Select((item) => SensorMapper.Map(item)) : null;
							if ((sensors != null) && (sensors.Any()))
							{
                                this.Cache.Set($"Sensor", sensors, this.MemoryCacheEntryOptions);
								result = ResultCode.Ok;
                            }
							else
							{
								result = ResultCode.ItemNotFound;
							}
						}
					}
				}
				finally
				{
					this.CacheSignal.Release();
				}
            }
        
			return (result, sensors);
		}
        #endregion
    }
}

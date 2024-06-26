﻿using Connect.Data.Entities;
using Connect.Data.Mappers;
using Connect.Data.Services.Repositories;
using Connect.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorSensor : Supervisor, ISupervisorSensor
    {
        private readonly Lazy<IRepository<SensorEntity>> _lazySensorRepository;

        #region Properties
        private IRepository<SensorEntity> SensorRepository => _lazySensorRepository.Value;
        #endregion

        #region Constructor
        public SupervisorSensor(IDalSession session, IRepositoryFactory repositoryFactory)
        {
            _lazySensorRepository = repositoryFactory.CreateRepository<SensorEntity>(session);
        }
        #endregion

        #region Methods
        public async Task<ResultCode> SensorExists(string id)
        {
            return (await this.SensorRepository.GetAsync(id) != null) ? ResultCode.Ok : ResultCode.ItemNotFound;
        }

        public async Task<IEnumerable<Sensor>> GetSensors()
        {
            IEnumerable<SensorEntity> entities = await this.SensorRepository.GetCollectionAsync();
            return entities.Select(item => SensorMapper.Map(item));
        }

        public async Task<Sensor> GetSensor(string id)
        {
            return SensorMapper.Map(await this.SensorRepository.GetAsync(id));
        }

        public async Task<ResultCode> AddSensor(Sensor sensor)
        {
            sensor.Id = string.IsNullOrEmpty(sensor.Id) ? Guid.NewGuid().ToString() : sensor.Id;
            int res = await this.SensorRepository.InsertAsync(SensorMapper.Map(sensor));
            ResultCode result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
            return result;
        }

        public async Task<Sensor> GetSensor(string type, string channel)
        {
            Sensor sensor = null;
            SensorEntity entity = await this.SensorRepository.GetAsync(arg => (arg.Name == type) && (arg.Channel == channel));
            if (entity != null) 
            {
                sensor = SensorMapper.Map(entity);
            }
            return sensor;
        }

        public async Task<(ResultCode, Sensor)> UpdateSensor(Sensor sensor)
        {
            ResultCode result = await this.SensorExists(sensor?.Id);
            if (result == ResultCode.Ok)
            {               
                int res = await this.SensorRepository.UpdateAsync(SensorMapper.Map(sensor));
                result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotUpdateItem;
            }

            return (result, sensor);
        }

        #endregion
    }
}

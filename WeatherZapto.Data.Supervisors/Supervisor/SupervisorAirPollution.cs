﻿using Framework.Core.Base;
using Framework.Data.Abstractions;
using WeatherZapto.Data.Entities;
using WeatherZapto.Data.Mappers;
using WeatherZapto.Data.Services.Repositories;
using WeatherZapto.Model;

namespace WeatherZapto.Data.Supervisors
{
    public partial class SupervisorAirPollution : ISupervisorAirPollution
    {
        private readonly Lazy<IRepository<AirPollutionEntity>> _lazyAirPollutionRepository;

        #region Properties
        private IRepository<AirPollutionEntity> AirPollutionRepository => _lazyAirPollutionRepository?.Value;
        #endregion

        #region Constructor
        public SupervisorAirPollution(IDalSession session, IRepositoryFactory repositoryFactory)
        {
            _lazyAirPollutionRepository = repositoryFactory?.CreateRepository<AirPollutionEntity>(session);
        }
        #endregion

        #region Methods
        public async Task<ResultCode> AirPollutionExists(string id)
        {
            return (await this.AirPollutionRepository.GetAsync(id) != null) ? ResultCode.Ok : ResultCode.ItemNotFound;
        }

        public async Task<ResultCode> AddAirPollutionAsync(ZaptoAirPollution airPollution)
        {
            ResultCode result = await this.AirPollutionExists(airPollution?.Id);
            if (result == ResultCode.ItemNotFound)
            {
                airPollution.Id = string.IsNullOrEmpty(airPollution.Id) ? Guid.NewGuid().ToString() : airPollution.Id;
                airPollution.Date = Clock.Now.ToUniversalTime();
                int res = await this.AirPollutionRepository.InsertAsync(AirPollutionMapper.Map(airPollution));
                result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
            }
            else
            {
                result = ResultCode.ItemAlreadyExist;
            }

            return result;
        }

        public async Task<ResultCode> DeleteAirPollutionAsync(ZaptoAirPollution airPollution)
        {
            return (await this.AirPollutionRepository.DeleteAsync(AirPollutionMapper.Map(airPollution)) > 0) ? ResultCode.Ok : ResultCode.CouldNotDeleteItem;
        }
        #endregion
    }
}

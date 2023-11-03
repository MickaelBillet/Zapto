﻿using Connect.Data.Entities;
using Connect.Data.Mappers;
using Connect.Data.Services.Repositories;
using Connect.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorLocation : ISupervisorLocation
    {
        private readonly Lazy<IRepository<LocationEntity>> _lazyLocationRepository;
        private readonly Lazy<IRepository<RoomEntity>> _lazyRoomRepository;

        #region Properties
        private IRepository<LocationEntity> LocationRepository => _lazyLocationRepository.Value;
        private IRepository<RoomEntity> RoomRepository => _lazyRoomRepository.Value;
        #endregion

        #region Constructor
        public SupervisorLocation(IDataContextFactory dataContextFactory, IRepositoryFactory repositoryFactory, IConfiguration configuration)
        {
            ConnectionType type = new ConnectionType()
            {
                ConnectionString = configuration["ConnectionStrings:DefaultConnection"],
                ServerType = ConnectionType.GetServerType(configuration["ConnectionStrings:ServerType"]),
            };

            IDataContext? context = dataContextFactory.CreateDbContext(type.ConnectionString, type.ServerType)?.context;
            if (context != null)
            {
                _lazyLocationRepository = repositoryFactory.CreateRepository<LocationEntity>(context);
                _lazyRoomRepository = repositoryFactory?.CreateRepository<RoomEntity>(context);
            }
        }
        #endregion

        #region Methods
        public async Task<ResultCode> LocationExists(string id)
        {
            return (await this.LocationRepository.GetAsync(id) != null) ? ResultCode.Ok : ResultCode.ItemNotFound;
        }

        /// <summary>
        /// Get Locations without children
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Location>> GetLocations()
        {
            IEnumerable<LocationEntity> entities = await this.LocationRepository.GetCollectionAsync();
            return entities.Select(item => LocationMapper.Map(item));   
        }

        /// <summary>
        /// Get Location with these rooms
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Location> GetLocation(string id)
        {
            Location location = LocationMapper.Map(await this.LocationRepository.GetAsync(id));
            if (location != null)
            {
                IEnumerable<RoomEntity> roomEntities = await this.RoomRepository.GetCollectionAsync((room) => room.LocationId == id);
                if (roomEntities != null)
                {
                    location.RoomsList = new ObservableCollection<Room>(roomEntities.Select(item => RoomMapper.Map(item)));
                }
            }

            return location;
        }

        public async Task<ResultCode> AddLocation(Location location)
        {
            location.Id = string.IsNullOrEmpty(location.Id) ? Guid.NewGuid().ToString() : location.Id;
            int res = await this.LocationRepository.InsertAsync(LocationMapper.Map(location));
            ResultCode result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
            return result;
        }

        #endregion
    }
}
using Connect.Data.Entities;
using Connect.Data.Mappers;
using Connect.Data.Services.Repositories;
using Connect.Data.Session;
using Connect.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorClientApps : ISupervisorClientApps
    {
        private readonly Lazy<IRepository<ClientAppEntity>> _lazyClientAppRepository;

        #region Properties
        private IRepository<ClientAppEntity> ClientAppRepository => _lazyClientAppRepository.Value;
        #endregion

        #region Constructor
        public SupervisorClientApps(IDalSession session, IRepositoryFactory repositoryFactory)
        {
            _lazyClientAppRepository = repositoryFactory.CreateRepository<ClientAppEntity>(session);
        }
        #endregion

        #region Methods
        public async Task<ResultCode> ClientAppExists(string id)
        {
            return (await this.ClientAppRepository.GetAsync(id) != null) ? ResultCode.Ok : ResultCode.ItemNotFound;
        }

        public async Task<IEnumerable<ClientApp>> GetClientApps()
        {
            IEnumerable<ClientAppEntity> entities = await this.ClientAppRepository.GetCollectionAsync();
            return (entities != null) ? entities.Select(item => ClientAppMapper.Map(item)) : Enumerable.Empty<ClientApp>();
        }

        public async Task<ClientApp> GetClientApp(string id)
        {
            ClientAppEntity entity = await this.ClientAppRepository.GetAsync(id);
            return (entity != null) ? ClientAppMapper.Map(entity) : null;
        }

        public async Task<ClientApp> GetClientAppFromToken(string token)
        {
            ClientAppEntity entity = await this.ClientAppRepository.GetAsync((item) => item.Token == token);
            return (entity != null) ? ClientAppMapper.Map(entity) : null;
        }

        public async Task<ResultCode> AddClientApp(ClientApp clientApp)
        {
            ResultCode result = ResultCode.CouldNotCreateItem;
            if (await this.GetClientAppFromToken(clientApp?.Token) == null)
            {
                clientApp.Id = string.IsNullOrEmpty(clientApp.Id) ? Guid.NewGuid().ToString() : clientApp.Id;
                int res = await this.ClientAppRepository.InsertAsync(ClientAppMapper.Map(clientApp));
                result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
            }
            else
            {
                result = ResultCode.ItemAlreadyExist;
            }

            return result;
        }

        public async Task<ResultCode> DeleteClientApp(ClientApp clientApp)
        {
            return (await this.ClientAppRepository.DeleteAsync(ClientAppMapper.Map(clientApp)) > 0) ? ResultCode.Ok : ResultCode.CouldNotDeleteItem;
        }

        public async Task<IEnumerable<ClientApp>> GetClienAppsFromLocation(string locationId)
        {
            IEnumerable<ClientAppEntity> entities = await this.ClientAppRepository.GetCollectionAsync((arg) => arg.LocationId == locationId);
            return (entities != null) ? entities.Select(item => ClientAppMapper.Map(item)) : Enumerable.Empty<ClientApp>();
        }
        #endregion
    }
}
using Connect.Data.Entities;
using Connect.Data.Mappers;
using Connect.Data.Services.Repositories;
using Connect.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorConfiguration : ISupervisorConfiguration
    {
        private readonly Lazy<IRepository<ConfigurationEntity>> _lazyConfigurationRepository;

        #region Properties
        private IRepository<ConfigurationEntity> ConfigurationRepository => _lazyConfigurationRepository.Value;
        #endregion

        #region Constructor
        public SupervisorConfiguration(IDataContextFactory dataContextFactory, IRepositoryFactory repositoryFactory, IConfiguration configuration)
        {
            ConnectionType type = new ConnectionType()
            {
                ConnectionString = configuration["ConnectionStrings:DefaultConnection"],
                ServerType = ConnectionType.GetServerType(configuration["ConnectionStrings:ServerType"]),
            };

            IDataContext? context = dataContextFactory.CreateDbContext(type.ConnectionString, type.ServerType)?.context;
            if (context != null)
            {
                _lazyConfigurationRepository = repositoryFactory.CreateRepository<ConfigurationEntity>(context);
            }
        }
        #endregion

        #region Methods
        public async Task<ResultCode> ConfigurationExists(string id)
        {
            return (await this.ConfigurationRepository.GetAsync(id) != null) ? ResultCode.Ok : ResultCode.ItemNotFound;
        }

        public async Task<ResultCode> AddConfiguration(Configuration configuration)
        {
            configuration.Id = string.IsNullOrEmpty(configuration.Id) ? Guid.NewGuid().ToString() : configuration.Id;
            int res = await this.ConfigurationRepository.InsertAsync(ConfigurationMapper.Map(configuration));
            ResultCode result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
            return result;
        }
        #endregion
    }
}

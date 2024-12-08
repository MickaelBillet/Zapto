using Connect.Data.Entities;
using Connect.Data.Mappers;
using Connect.Data.Services.Repositories;
using Connect.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorConfiguration : Supervisor, ISupervisorConfiguration
    {
        private readonly Lazy<IRepository<ConfigurationEntity>> _lazyConfigurationRepository;

        #region Properties
        private IRepository<ConfigurationEntity> ConfigurationRepository => _lazyConfigurationRepository.Value;
        #endregion

        #region Constructor
        public SupervisorConfiguration(IDalSession session, IRepositoryFactory repositoryFactory)
        {
            _lazyConfigurationRepository = repositoryFactory.CreateRepository<ConfigurationEntity>(session);
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
        public async Task<IEnumerable<Configuration>> GetConfigurations()
        {
            return (await this.ConfigurationRepository.GetCollectionAsync()).Select((arg) => ConfigurationMapper.Map(arg));
        }
        #endregion
    }
}

using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorCacheConfiguration : SupervisorCache, ISupervisorConfiguration
    {
        #region Services
        private ISupervisorConfiguration Supervisor { get; }
        #endregion

        #region Constructor
        public SupervisorCacheConfiguration(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.Supervisor = serviceProvider.GetRequiredService<ISupervisorFactoryConfiguration>().CreateSupervisor(CacheType.None);
        }
        #endregion

        #region Methods
        public async Task Initialize()
        {
            IEnumerable<Configuration> configurations = await this.Supervisor.GetConfigurations();
            foreach (var item in configurations)
            {
                await this.CacheConfigurationService.Set(item.Id, item);
            }
        }
        public async Task<IEnumerable<Configuration>> GetConfigurations()
        {
            return await this.Supervisor.GetConfigurations();
        }

        public async Task<ResultCode> AddConfiguration(Configuration configuration)
        {
            ResultCode code = await this.Supervisor.AddConfiguration(configuration);
            if (code == ResultCode.Ok)
            {
                await this.CacheConfigurationService.Set(configuration.Id, configuration);
            }
            return code;
        }
        #endregion
    }
}

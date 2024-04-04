using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorCacheConfiguration : SupervisorCache
    {
        #region Services
        private ISupervisorConfiguration Supervisor { get; }
        #endregion

        #region Constructor
        public SupervisorCacheConfiguration(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.Supervisor = serviceProvider.GetRequiredService<ISupervisorConfiguration>();
        }
        #endregion

        #region Methods
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

using Connect.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorFactoryConfiguration : ISupervisorFactoryConfiguration
    {
        #region Services
        private IServiceProvider ServiceProvider { get; }
        private IDalSession Session { get; }
        private IRepositoryFactory RepositoryFactory { get; }
        #endregion

        #region Constructor
        public SupervisorFactoryConfiguration(IServiceProvider serviceProvider) 
        {
            this.ServiceProvider = serviceProvider;
            this.Session = ServiceProvider.GetService<IDalSession>();
            this.RepositoryFactory = ServiceProvider.GetService<IRepositoryFactory>();
        }
        #endregion

        #region Methods
        public ISupervisorConfiguration CreateSupervisor(int? cacheIsHandled)
        {
            if ((cacheIsHandled != null) && (cacheIsHandled is 1)) 
            {
                return new SupervisorCacheConfiguration(this.ServiceProvider);
            }
            else
            {
                return new SupervisorConfiguration(this.Session, this.RepositoryFactory);
            }
        }
        #endregion
    }
}

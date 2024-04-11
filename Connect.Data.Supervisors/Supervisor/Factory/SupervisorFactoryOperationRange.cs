using Connect.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorFactoryOperationRange : ISupervisorFactoryOperationRange
    {
        #region Services
        private IServiceProvider ServiceProvider { get; }
        private IDalSession Session { get; }
        private IRepositoryFactory RepositoryFactory { get; }
        #endregion

        #region Constructor
        public SupervisorFactoryOperationRange(IServiceProvider serviceProvider) 
        {
            this.ServiceProvider = serviceProvider;
            this.Session = serviceProvider.GetService<IDalSession>();
            this.RepositoryFactory = serviceProvider.GetService<IRepositoryFactory>();
        }
        #endregion

        #region Methods
        public ISupervisorOperationRange CreateSupervisor(int? cacheIsHandled)
        {
            if ((cacheIsHandled != null) && (cacheIsHandled is 1)) 
            {
                return new SupervisorCacheOperationRange(this.ServiceProvider);
            }
            else
            {
                return new SupervisorOperationRange(this.Session, this.RepositoryFactory);
            }
        }
        #endregion
    }
}

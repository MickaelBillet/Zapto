using Connect.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorFactoryLocation : ISupervisorFactoryLocation
    {
        #region Services
        private IServiceProvider ServiceProvider { get; }
        private IDalSession Session { get; }
        private IRepositoryFactory RepositoryFactory { get; }
        #endregion

        #region Constructor
        public SupervisorFactoryLocation(IServiceProvider serviceProvider) 
        {
            this.ServiceProvider = serviceProvider;
            this.Session = serviceProvider.GetRequiredService<IDalSession>();
            this.RepositoryFactory = serviceProvider.GetRequiredService<IRepositoryFactory>();
        }
        #endregion

        #region Methods
        public ISupervisorLocation CreateSupervisor(int? cacheIsHandled)
        {
            if ((cacheIsHandled != null) && (cacheIsHandled is 1)) 
            {
                return new SupervisorCacheLocation(this.ServiceProvider);
            }
            else
            {
                return new SupervisorLocation(this.Session, this.RepositoryFactory);
            }
        }
        #endregion
    }
}

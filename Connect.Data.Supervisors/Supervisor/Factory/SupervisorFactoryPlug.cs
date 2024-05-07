using Connect.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorFactoryPlug : ISupervisorFactoryPlug
    {
        #region Services
        private IServiceProvider ServiceProvider { get; }
        private IDalSession Session { get; }
        private IRepositoryFactory RepositoryFactory { get; }
        #endregion

        #region Constructor
        public SupervisorFactoryPlug(IServiceProvider serviceProvider) 
        {
            this.ServiceProvider = serviceProvider;
            this.Session = serviceProvider.GetService<IDalSession>();
            this.RepositoryFactory = serviceProvider.GetService<IRepositoryFactory>();
        }
        #endregion

        #region Methods
        public ISupervisorPlug CreateSupervisor(int? cacheIsHandled)
        {
            if ((cacheIsHandled != null) && (cacheIsHandled is 1))
            {
                return new SupervisorCachePlug(this.ServiceProvider);
            }
            else
            {
                return new SupervisorPlug(this.Session, this.RepositoryFactory);
            }
        }
        #endregion
    }
}

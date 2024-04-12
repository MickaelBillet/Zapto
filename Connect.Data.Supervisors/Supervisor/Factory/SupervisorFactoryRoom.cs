using Connect.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorFactoryRoom : ISupervisorFactoryRoom
    {
        #region Services
        private IServiceProvider ServiceProvider { get; }
        private IDalSession Session { get; }
        private IRepositoryFactory RepositoryFactory { get; }
        #endregion

        #region Constructor
        public SupervisorFactoryRoom(IServiceProvider serviceProvider) 
        {
            this.ServiceProvider = serviceProvider;
            this.Session = serviceProvider.GetService<IDalSession>();
            this.RepositoryFactory = serviceProvider.GetService<IRepositoryFactory>();
        }
        #endregion

        #region Methods
        public ISupervisorRoom CreateSupervisor(byte? cacheIsHandled)
        {
            if ((cacheIsHandled != null) && (cacheIsHandled is 1))
            {
                return new SupervisorCacheRoom(this.ServiceProvider);
            }
            else
            {
                return new SupervisorRoom(this.Session, this.RepositoryFactory);
            }
        }
        #endregion
    }
}

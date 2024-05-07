using Connect.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorFactoryProgram : ISupervisorFactoryProgram
    {
        #region Services
        private IServiceProvider ServiceProvider { get; }
        private IDalSession Session { get; }
        private IRepositoryFactory RepositoryFactory { get; }
        #endregion

        #region Constructor
        public SupervisorFactoryProgram(IServiceProvider serviceProvider) 
        {
            this.ServiceProvider = serviceProvider;
            this.Session = serviceProvider.GetService<IDalSession>();
            this.RepositoryFactory = serviceProvider.GetService<IRepositoryFactory>();
        }
        #endregion

        #region Methods
        public ISupervisorProgram CreateSupervisor(int? cacheIsHandled)
        {
            if ((cacheIsHandled != null) && (cacheIsHandled is 1))
            {
                return new SupervisorCacheProgram(this.ServiceProvider);
            }
            else
            {
                return new SupervisorProgram(this.Session, this.RepositoryFactory);
            }
        }
        #endregion
    }
}

using Connect.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorFactoryClientApp : ISupervisorFactoryClientApp
    {
        #region Services
        private IDalSession Session { get; }
        private IRepositoryFactory RepositoryFactory { get; }
        #endregion

        #region Constructor
        public SupervisorFactoryClientApp(IServiceProvider serviceProvider) 
        {
            this.Session = serviceProvider.GetRequiredService<IDalSession>();
            this.RepositoryFactory = serviceProvider.GetRequiredService<IRepositoryFactory>();
        }
        #endregion

        #region Methods
        public ISupervisorClientApps CreateSupervisor()
        {
            return new SupervisorClientApps(this.Session, this.RepositoryFactory);
        }
        #endregion
    }
}

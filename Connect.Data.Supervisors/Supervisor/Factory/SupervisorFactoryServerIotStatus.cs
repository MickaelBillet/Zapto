using Connect.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorFactoryServerIotStatus : ISupervisorFactoryServerIotStatus
    {
        #region Services
        private IDalSession Session { get; }
        private IRepositoryFactory RepositoryFactory { get; }
        #endregion

        #region Constructor
        public SupervisorFactoryServerIotStatus(IServiceProvider serviceProvider) 
        {
            this.Session = serviceProvider.GetRequiredService<IDalSession>();
            this.RepositoryFactory = serviceProvider.GetRequiredService<IRepositoryFactory>();
        }
        #endregion

        #region Methods
        public ISupervisorServerIotStatus CreateSupervisor()
        {
            return new SupervisorServerIotStatus(this.Session, this.RepositoryFactory);
        }
        #endregion
    }
}

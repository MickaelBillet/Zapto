using Connect.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorFactoryLog : ISupervisorFactoryLog
    {
        #region Services
        private IDalSession Session { get; }
        private IRepositoryFactory RepositoryFactory { get; }
        #endregion

        #region Constructor
        public SupervisorFactoryLog(IServiceProvider serviceProvider) 
        {
            this.Session = serviceProvider.GetService<IDalSession>();
            this.RepositoryFactory = serviceProvider.GetService<IRepositoryFactory>();
        }
        #endregion

        #region Methods
        public ISupervisorLog CreateSupervisor()
        {
            return new SupervisorLog(this.Session, this.RepositoryFactory);
        }
        #endregion
    }
}

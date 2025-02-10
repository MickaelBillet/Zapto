using Connect.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorFactoryVersion : ISupervisorFactoryVersion
    {
        #region Services
        private IDalSession Session { get; }
        private IRepositoryFactory RepositoryFactory { get; }
        #endregion

        #region Constructor
        public SupervisorFactoryVersion(IServiceProvider serviceProvider) 
        {
            this.Session = serviceProvider.GetRequiredService<IDalSession>();
            this.RepositoryFactory = serviceProvider.GetRequiredService<IRepositoryFactory>();
        }
        #endregion

        #region Methods
        public ISupervisorVersion CreateSupervisor()
        {
            return new SupervisorVersion(this.Session, this.RepositoryFactory);
        }
        #endregion
    }
}

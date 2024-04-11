using Connect.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorFactoryOperatingData : ISupervisorFactoryOperatingData
    {
        #region Services
        private IDalSession Session { get; }
        private IRepositoryFactory RepositoryFactory { get; }
        #endregion

        #region Constructor
        public SupervisorFactoryOperatingData(IServiceProvider serviceProvider) 
        {
            this.Session = serviceProvider.GetService<IDalSession>();
            this.RepositoryFactory = serviceProvider.GetService<IRepositoryFactory>();
        }
        #endregion

        #region Methods
        public ISupervisorOperatingData CreateSupervisor()
        {
            return new SupervisorOperatingData(this.Session, this.RepositoryFactory);
        }
        #endregion
    }
}

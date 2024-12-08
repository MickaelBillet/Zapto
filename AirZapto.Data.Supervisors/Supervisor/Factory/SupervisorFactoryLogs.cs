using AirZapto.Data.Services;
using AirZapto.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace AirZapto.Data.Supervisors
{
    public sealed class SupervisorFactoryLogs : ISupervisorFactoryLogs
    {
        #region Services
        private IDalSession? Session { get; }
        private IRepositoryFactory? RepositoryFactory { get; }
        private IDataContextFactory? ContextFactory { get; }
        #endregion

        #region Constructor
        public SupervisorFactoryLogs(IServiceProvider serviceProvider)
        {
            this.Session = serviceProvider.GetService<IDalSession>();
            this.ContextFactory = serviceProvider.GetService<IDataContextFactory>();
            this.RepositoryFactory = serviceProvider.GetService<IRepositoryFactory>();
        }
        #endregion

        #region Methods
        public ISupervisorLogs CreateSupervisor()
        {
            return new SupervisorLogs(this.Session!, this.ContextFactory!, this.RepositoryFactory!);
        }
        #endregion
    }
}

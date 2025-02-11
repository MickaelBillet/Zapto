using AirZapto.Data.Services;
using AirZapto.Data.Services.Repositories;
using Framework.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace AirZapto.Data.Supervisors
{
    public sealed class SupervisorFactoryVersion : ISupervisorFactoryVersion
    {
        #region Services
        private IDalSession? Session { get; }
        private IRepositoryFactory? RepositoryFactory { get; }
        #endregion

        #region Constructor
        public SupervisorFactoryVersion(IServiceProvider serviceProvider)
        {
            this.Session = serviceProvider.GetService<IDalSession>();
            this.RepositoryFactory = serviceProvider.GetService<IRepositoryFactory>();
        }
        #endregion

        #region Methods
        public ISupervisorVersion CreateSupervisor()
        {
            return new SupervisorVersion(this.Session!, this.RepositoryFactory!);
        }
        #endregion
    }
}

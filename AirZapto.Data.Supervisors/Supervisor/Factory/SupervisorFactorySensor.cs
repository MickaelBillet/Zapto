using AirZapto.Data.Services.Repositories;
using AirZapto.Model;
using Framework.Data.Abstractions;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AirZapto.Data.Supervisors
{
    public sealed class SupervisorFactorySensor : ISupervisorFactorySensor
    {
        #region Services
        private IDalSession Session { get; }
        private IRepositoryFactory RepositoryFactory { get; }
        private IConfiguration Configuration { get; }
        private ICacheZaptoService<Sensor> CacheService { get; }
        #endregion

        #region Constructor
        public SupervisorFactorySensor(IServiceProvider serviceProvider)
        {
            this.Session = serviceProvider.GetRequiredService<IDalSession>();
            this.RepositoryFactory = serviceProvider.GetRequiredService<IRepositoryFactory>();
            this.Configuration = serviceProvider.GetRequiredService<IConfiguration>();
            this.CacheService = serviceProvider.GetRequiredService<ICacheZaptoService<Sensor>>();
        }
        #endregion

        #region Methods
        public ISupervisorSensor CreateSupervisor()
        {
            ISupervisorSensor? supervisor = null;

            if ((int.TryParse(this.Configuration["Cache"], out int cache) == true) && (cache == 1))
            {
                supervisor = new SupervisorCacheSensor(this.Session, this.RepositoryFactory, this.CacheService);
            }
            else
            {
                supervisor = new SupervisorSensor(this.Session, this.RepositoryFactory);
            }
            return supervisor;
        }
        #endregion
    }
}

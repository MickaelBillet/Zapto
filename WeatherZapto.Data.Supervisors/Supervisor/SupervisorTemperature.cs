using Framework.Data.Abstractions;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WeatherZapto.Data.Entities;
using WeatherZapto.Data.Services.Repositories;
using WeatherZapto.Model;
using WeatherZapto.Model.Healthcheck;

namespace WeatherZapto.Data.Supervisors
{
    public class SupervisorTemperature : ISupervisorTemperature
    {
        private readonly Lazy<IRepository<WeatherEntity>> _lazyWeatherRepository;

        #region Services
        private IRepository<WeatherEntity> WeatherRepository => _lazyWeatherRepository?.Value;
        #endregion

        #region Constructor
        public SupervisorTemperature(IServiceProvider serviceProvider)
        {
            IRepositoryFactory repositoryFactory = serviceProvider.GetRequiredService<IRepositoryFactory>();
            IDalSession session = serviceProvider.GetRequiredService<IDalSession>();

            _lazyWeatherRepository = repositoryFactory?.CreateRepository<WeatherEntity>(session);
        }
        #endregion

        #region Methods
        
        public async Task<IEnumerable<double>> GetTemperatures(string location, DateTime day)
        {
            IEnumerable<double> temperatures = null;
            if (this.WeatherRepository != null)
            {
                temperatures = (await this.WeatherRepository.GetCollectionAsync((item) => item.CreationDateTime.ToUniversalTime().Date.Equals(day.ToUniversalTime().Date)
                                                                                            && item.Location.Equals(location)))
                                                            .Select((item) => double.Parse(item.Temperature));
            }
            return temperatures;
        }

        public async Task<double?> GetTemperatureMin(string location, DateTime day)
        {
            return (await this.WeatherRepository.GetCollectionAsync((item) => item.CreationDateTime.ToUniversalTime().Date.Year.Equals(day.Year)
                                                                            && item.CreationDateTime.ToUniversalTime().Date.Month.Equals(day.Month)
                                                                            && item.CreationDateTime.ToUniversalTime().Date.Day.Equals(day.Day)
                                                                            && item.Location.Equals(location)))
                                                .Min((item) => float.Parse(item.Temperature));
        }

        public async Task<double?> GetTemperatureMax(string location, DateTime day)
        {
            return (await this.WeatherRepository.GetCollectionAsync((item) => item.CreationDateTime.ToUniversalTime().Date.Year.Equals(day.Year)
                                                                            && item.CreationDateTime.ToUniversalTime().Date.Month.Equals(day.Month)
                                                                            && item.CreationDateTime.ToUniversalTime().Date.Day.Equals(day.Day)
                                                                            && item.Location.Equals(location)))
                                                .Max((item) => float.Parse(item.Temperature));
        }
        #endregion
    }
}

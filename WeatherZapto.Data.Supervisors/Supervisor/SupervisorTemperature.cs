using Framework.Data.Abstractions;
using WeatherZapto.Data.Entities;
using WeatherZapto.Data.Services.Repositories;

namespace WeatherZapto.Data.Supervisors
{
    public class SupervisorTemperature : ISupervisorTemperature
    {
        private readonly Lazy<IRepository<WeatherEntity>> _lazyWeatherRepository;

        #region Properties
        private IRepository<WeatherEntity> WeatherRepository => _lazyWeatherRepository?.Value;
        #endregion

        #region Constructor
        public SupervisorTemperature(IDalSession session, IRepositoryFactory repositoryFactory)
        {
            _lazyWeatherRepository = repositoryFactory?.CreateRepository<WeatherEntity>(session);
        }
        #endregion

        #region Methods
        
        public async Task<IEnumerable<float>> GetTemperatures(string location, DateTime day)
        {
            IEnumerable<float> temperatures = null;
            if (this.WeatherRepository != null)
            {
                temperatures = (await this.WeatherRepository.GetCollectionAsync((item) => item.CreationDateTime.ToUniversalTime().Date.Equals(day.ToUniversalTime().Date)
                                                                                            && item.Location.Equals(location)))
                                                            .Select((item) => float.Parse(item.Temperature));
            }
            return temperatures;
        }

        public async Task<float?> GetTemperatureMin(string location, DateTime day)
        {
            float? min = null;
            if (this.WeatherRepository != null)
            {
                min = (await this.WeatherRepository.GetCollectionAsync((item) => item.CreationDateTime.ToUniversalTime().Date.Equals(day.ToUniversalTime().Date)
                                                                                                    && item.Location.Equals(location)))
                                                   .Min((item) => float.Parse(item.Temperature));
            }
            return min;
        }

        public async Task<float?> GetTemperatureMax(string location, DateTime day)
        {
            float? max = null;
            if (this.WeatherRepository != null)
            {
                max = (await this.WeatherRepository.GetCollectionAsync((item) => item.CreationDateTime.ToUniversalTime().Date.Equals(day.ToUniversalTime().Date)
                                                                                                    && item.Location.Equals(location)))
                                                   .Max((item) => float.Parse(item.Temperature));
            }
            return max;
        }
        #endregion
    }
}

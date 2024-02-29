using Framework.Data.Abstractions;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WeatherZapto.Data.Entities;
using WeatherZapto.Data.Services.Repositories;

namespace WeatherZapto.Data.Supervisors
{
    public class SupervisorTemperature : ISupervisorTemperature
    {
        private readonly Lazy<IRepository<WeatherEntity>> _lazyWeatherRepository;

        #region Services
        private IRepository<WeatherEntity> WeatherRepository => _lazyWeatherRepository?.Value;
        private IMemoryCache Cache { get; }
        private CacheSignal CacheSignal { get; }
        private MemoryCacheEntryOptions MemoryCacheEntryOptions { get; }
        #endregion

        #region Constructor
        public SupervisorTemperature(IServiceProvider serviceProvider)
        {
            IRepositoryFactory repositoryFactory = serviceProvider.GetRequiredService<IRepositoryFactory>();
            IDalSession session = serviceProvider.GetRequiredService<IDalSession>();
            this.Cache = serviceProvider.GetService<IMemoryCache>();
            this.CacheSignal = serviceProvider.GetService<CacheSignal>();
            this.MemoryCacheEntryOptions = new MemoryCacheEntryOptions()
                                                       .SetSlidingExpiration(TimeSpan.FromSeconds(300)) //This determines how long a cache entry can be inactive before it is removed from the cache
                                                       .SetAbsoluteExpiration(TimeSpan.FromSeconds(900)) //The problem with sliding expiration is that if we keep on accessing the cache entry, it will never expire
                                                       .SetPriority(CacheItemPriority.Normal);

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
            double? temperatureMin = null;
            if (this.CacheSignal != null && this.Cache != null)
            {
                try
                {
                    await this.CacheSignal.WaitAsync();
                    if (this.Cache.TryGetValue($"TemperatureMin-{location}", out temperatureMin))
                    {
                        Log.Information($"TemperatureMin found {location}");
                    }
                    else
                    {
                        if (this.WeatherRepository != null)
                        {
                            IEnumerable<WeatherEntity> entities = await this.WeatherRepository.GetCollectionAsync((item) => item.CreationDateTime.ToUniversalTime().Date.Year.Equals(day.Year)
                                                                                                        && item.CreationDateTime.ToUniversalTime().Date.Month.Equals(day.Month)
                                                                                                        && item.CreationDateTime.ToUniversalTime().Date.Day.Equals(day.Day)
                                                                                                        && item.Location.Equals(location));

                            if ((entities != null) && (entities.Any()))
                            {
                                temperatureMin = entities.Min((item) => float.Parse(item.Temperature));
                                this.Cache.Set($"TemperatureMin-{location}", temperatureMin, this.MemoryCacheEntryOptions);
                            }
                        }
                    }
                }
                finally
                {
                    this.CacheSignal.Release();
                }
            }
            return temperatureMin;
        }

        public async Task<double?> GetTemperatureMax(string location, DateTime day)
        {
            double? temperatureMax = null;
            if (this.CacheSignal != null && this.Cache != null)
            {
                try
                {
                    await this.CacheSignal.WaitAsync();
                    if (this.Cache.TryGetValue($"TemperatureMax-{location}", out temperatureMax))
                    {
                        Log.Information($"TemperatureMax found {location}");
                    }
                    else
                    {
                        if (this.WeatherRepository != null)
                        {
                            IEnumerable<WeatherEntity> entities = await this.WeatherRepository.GetCollectionAsync((item) => item.CreationDateTime.ToUniversalTime().Date.Year.Equals(day.Year)
                                                                                && item.CreationDateTime.ToUniversalTime().Date.Month.Equals(day.Month)
                                                                                && item.CreationDateTime.ToUniversalTime().Date.Day.Equals(day.Day)
                                                                                && item.Location.Equals(location));

                            if ((entities != null) && (entities.Any()))
                            { 
                                temperatureMax = entities.Max((item) => float.Parse(item.Temperature));
                                this.Cache.Set($"TemperatureMax-{location}", temperatureMax, this.MemoryCacheEntryOptions);
                            }
                        }
                    }
                }
                finally
                {
                    this.CacheSignal.Release();
                }
            }
            return temperatureMax;
        }
        #endregion
    }
}

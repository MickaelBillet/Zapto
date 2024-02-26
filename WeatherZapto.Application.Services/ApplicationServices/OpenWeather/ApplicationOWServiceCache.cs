using Framework.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WeatherZapto.Data;
using WeatherZapto.Model;

namespace WeatherZapto.Application.Services
{
    internal class ApplicationOWServiceCache : IApplicationOWServiceCache
    {
        #region Services
        private IApplicationOWService ApplicationOWService { get; }
        private IMemoryCache Cache { get; }
        private CacheSignal CacheSignal { get; }
        private MemoryCacheEntryOptions MemoryCacheEntryOptions { get; }
        private ISupervisorCall SupervisorCall { get; }
        #endregion

        #region Constructor
        public ApplicationOWServiceCache(IServiceProvider serviceProvider)
        {
            SupervisorCall = serviceProvider.GetService<ISupervisorCall>();
            ApplicationOWService = serviceProvider.GetService<IApplicationOWService>();
            Cache = serviceProvider.GetService<IMemoryCache>();
            CacheSignal = serviceProvider.GetService<CacheSignal>();
            MemoryCacheEntryOptions = new MemoryCacheEntryOptions()
                                                       .SetSlidingExpiration(TimeSpan.FromSeconds(300)) //This determines how long a cache entry can be inactive before it is removed from the cache
                                                       .SetAbsoluteExpiration(TimeSpan.FromSeconds(900)) //The problem with sliding expiration is that if we keep on accessing the cache entry, it will never expire
                                                       .SetPriority(CacheItemPriority.Normal);
        }
        #endregion

        #region Methods
        public async Task<ZaptoAirPollution> GetCurrentAirPollution(string APIKey, string locationName, string longitude, string latitude)
        {
            ZaptoAirPollution zaptoAirPollution = null;
            if (CacheSignal != null && Cache != null)
            {
                try
                {
                    await CacheSignal.WaitAsync();
                    if (Cache.TryGetValue($"AirPollution-{locationName}", out zaptoAirPollution))
                    {
                        Log.Information("AirPollution found");
                    }
                    else
                    {
                        zaptoAirPollution = await ApplicationOWService.GetCurrentAirPollution(APIKey, locationName, longitude, latitude);
                        if (zaptoAirPollution != null)
                        {
                            await SupervisorCall.AddCallOpenWeather();
                            Cache.Set($"AirPollution-{locationName}", zaptoAirPollution, MemoryCacheEntryOptions);
                        }
                    }
                }
                finally
                {
                    CacheSignal.Release();
                }
            }
            return zaptoAirPollution;
        }

        public async Task<ZaptoWeather> GetCurrentWeather(string APIKey, string locationName, string longitude, string latitude, string language)
        {
            ZaptoWeather zaptoWeather = null;
            if (CacheSignal != null && Cache != null)
            {
                try
                {
                    await CacheSignal.WaitAsync();
                    if (Cache.TryGetValue($"OpenWeather-{locationName}", out zaptoWeather))
                    {
                        Log.Information("OpenWeather found");
                    }
                    else
                    {
                        zaptoWeather = await ApplicationOWService.GetCurrentWeather(APIKey, locationName, longitude, latitude, language);
                        if (zaptoWeather != null)
                        {
                            await SupervisorCall.AddCallOpenWeather();
                            Cache.Set($"OpenWeather-{locationName}", zaptoWeather, MemoryCacheEntryOptions);
                        }
                    }
                }
                finally
                {
                    CacheSignal.Release();
                }
            }
            return zaptoWeather;
        }
        #endregion
    }
}

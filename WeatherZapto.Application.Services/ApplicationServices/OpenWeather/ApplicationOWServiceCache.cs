using Framework.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;
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
            this.SupervisorCall = serviceProvider.GetService<ISupervisorCall>();
            this.ApplicationOWService = serviceProvider.GetService<IApplicationOWService>();
            this.Cache = serviceProvider.GetService<IMemoryCache>();
            this.CacheSignal = serviceProvider.GetService<CacheSignal>();
            this.MemoryCacheEntryOptions = new MemoryCacheEntryOptions()
                                                       .SetSlidingExpiration(TimeSpan.FromSeconds(300)) //This determines how long a cache entry can be inactive before it is removed from the cache
                                                       .SetAbsoluteExpiration(TimeSpan.FromSeconds(900)) //The problem with sliding expiration is that if we keep on accessing the cache entry, it will never expire
                                                       .SetPriority(CacheItemPriority.Normal);
        }
        #endregion

        #region Methods
        public async Task<ZaptoAirPollution> GetCurrentAirPollution(string locationName, string longitude, string latitude)
        {
            ZaptoAirPollution zaptoAirPollution = null;
            if (this.CacheSignal != null && this.Cache != null)
            {
                try
                {
                    await this.CacheSignal.WaitAsync();
                    if (this.Cache.TryGetValue($"AirPollution-{locationName}", out zaptoAirPollution))
                    {
                        Log.Information("AirPollution found");
                    }
                    else
                    {
                        zaptoAirPollution = await this.ApplicationOWService.GetCurrentAirPollution(locationName, longitude, latitude);
                        if (zaptoAirPollution != null)
                        {
                            await this.SupervisorCall.AddCallOpenWeather();
                            this.Cache.Set($"AirPollution-{locationName}", zaptoAirPollution, this.MemoryCacheEntryOptions);
                        }
                    }
                }
                finally
                {
                    this.CacheSignal.Release();
                }
            }
            return zaptoAirPollution;
        }

        public async Task<ZaptoWeather> GetCurrentWeather(string locationName, string longitude, string latitude, string language)
        {
            ZaptoWeather zaptoWeather = null;
            if (this.CacheSignal != null && this.Cache != null)
            {
                try
                {
                    await this.CacheSignal.WaitAsync();
                    if (this.Cache.TryGetValue($"OpenWeather-{locationName}", out zaptoWeather))
                    {
                        Log.Information("OpenWeather found");
                    }
                    else
                    {
                        zaptoWeather = await this.ApplicationOWService.GetCurrentWeather(locationName, longitude, latitude, language);
                        if (zaptoWeather != null)
                        {
                            await this.SupervisorCall.AddCallOpenWeather();
                            this.Cache.Set($"OpenWeather-{locationName}", zaptoWeather, this.MemoryCacheEntryOptions);
                        }
                    }
                }
                finally
                {
                    this.CacheSignal.Release();
                }
            }
            return zaptoWeather;
        }
        #endregion
    }
}

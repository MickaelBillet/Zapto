using Framework.Core.Base;
using Framework.Data.Abstractions;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherZapto.Application;
using WeatherZapto.Data;
using WeatherZapto.Model;

namespace WeatherZapto.WebServer.Services
{
    public sealed class HomeAirPollutionAcquisitionService : BackgroundService
    {
        private bool _isCacheInitialized = false;
        private readonly TimeSpan _updateInterval = TimeSpan.FromMinutes(15);

        #region Services
        private CacheSignal CacheSignal { get; }
        private IConfiguration Configuration { get; }
        private IApplicationOWService ApplicationOWService { get; }
        private IMemoryCache Cache { get; }
        private IServiceScopeFactory ServiceScopeFactory { get; }
        private IDatabaseService DatabaseService { get; }
        private MemoryCacheEntryOptions CacheEntryOptions { get; }
        #endregion

        #region Constructor
        public HomeAirPollutionAcquisitionService(IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base()
        {
            this.CacheSignal = serviceProvider?.GetRequiredService<CacheSignal>();
            this.Configuration = configuration;
            this.ApplicationOWService = serviceProvider?.GetRequiredService<IApplicationOWService>();
            this.Cache = serviceProvider?.GetRequiredService<IMemoryCache>();
            this.DatabaseService = serviceProvider?.GetRequiredService<IDatabaseService>();
            this.ServiceScopeFactory = serviceScopeFactory;
            this.CacheEntryOptions = new MemoryCacheEntryOptions()
                                            .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(900))
                                            .SetPriority(CacheItemPriority.Normal);
        }
        #endregion

        #region Methods
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            if (this.CacheSignal != null)
            {
                await this.CacheSignal.WaitAsync();
                await base.StartAsync(cancellationToken);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    ZaptoAirPollution zaptoAirPollution = await this.ApplicationOWService.GetCurrentAirPollution(this.Configuration["OpenWeatherAPIKey"],
                                                                                                                            this.Configuration["HomeLocation"],
                                                                                                                            this.Configuration["HomeLongitude"],
                                                                                                                            this.Configuration["HomeLatitude"]);
                    if (zaptoAirPollution != null) 
                    {
                        using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
                        {
                            if (this.DatabaseService.IsInitialized == true)
                            {
                                ResultCode code = await scope.ServiceProvider.GetRequiredService<ISupervisorAirPollution>().AddAirPollutionAsync(zaptoAirPollution);
                                if (code == ResultCode.Ok)
                                {
                                    this.Cache.Set<ZaptoAirPollution>($"AirPollution-{this.Configuration["HomeLocation"]}", zaptoAirPollution, this.CacheEntryOptions);
                                }
                            }
                        }
                    }
                }
                finally
                {
                    if ((_isCacheInitialized == false) && (this.CacheSignal != null))
                    {
                        this.CacheSignal.Release();
                        _isCacheInitialized = true;
                    }
                }

                try
                {
                    await Task.Delay(_updateInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
        #endregion
    }
}

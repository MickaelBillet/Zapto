using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using WeatherZapto.Application;
using WeatherZapto.Data;
using WeatherZapto.Model;

namespace WeatherZapto.WebServer.Services
{
    public sealed class HomeAirPollutionAcquisitionService : BackgroundService
    {
        private readonly TimeSpan _updateInterval = TimeSpan.FromMinutes(15);

        #region Services
        private IConfiguration Configuration { get; }
        private IApplicationOWService ApplicationOWService { get; }
        private IServiceScopeFactory ServiceScopeFactory { get; }
        private IDatabaseService DatabaseService { get; }
        #endregion

        #region Constructor
        public HomeAirPollutionAcquisitionService(IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base()
        {
            this.Configuration = configuration;
            this.ApplicationOWService = serviceProvider?.GetRequiredService<IApplicationOWService>();
            this.DatabaseService = serviceProvider?.GetRequiredService<IDatabaseService>();
            this.ServiceScopeFactory = serviceScopeFactory;
        }
        #endregion

        #region Methods
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested == false)
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
                                if (code != ResultCode.CouldNotCreateItem)
                                {
                                    Log.Error("Error : No Storage for AirPollution");
                                }
                            }
                        }
                    }

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

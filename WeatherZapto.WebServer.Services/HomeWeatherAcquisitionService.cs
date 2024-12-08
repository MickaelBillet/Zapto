using Framework.Core.Base;
using Framework.Infrastructure.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using WeatherZapto.Application;
using WeatherZapto.Data;
using WeatherZapto.Model;

namespace WeatherZapto.WebServer.Services
{
    public sealed class HomeWeatherAcquisitionService : BackgroundService
    {
        #region Properties
        private TimeSpan AcquisitionPeriod { get; init; }
        #endregion

        #region Services
        private IConfiguration Configuration { get; }
        private IApplicationOWService ApplicationOWService { get; }
        private IServiceScopeFactory ServiceScopeFactory { get; }
        private IDatabaseService DatabaseService { get; }
        #endregion

        #region Constructor
        public HomeWeatherAcquisitionService(IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base()
        {
            this.Configuration = configuration;
            this.ApplicationOWService = serviceProvider?.GetRequiredService<IApplicationOWService>();
            this.DatabaseService = serviceProvider?.GetRequiredService<IDatabaseService>();
            this.ServiceScopeFactory = serviceScopeFactory;
            this.AcquisitionPeriod = new TimeSpan(0, int.Parse(this.Configuration["AcquisitionPeriod"]), 0);
        }
        #endregion

        #region Methods
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested == false)
            {
                try
                {
                    ZaptoWeather zaptoWeather = await this.ApplicationOWService.GetCurrentWeather(this.Configuration["HomeLocation"],
                                                                                                        this.Configuration["HomeLongitude"],
                                                                                                        this.Configuration["HomeLatitude"],
                                                                                                        "fr");
                    if (zaptoWeather != null)
                    {
                        using (IServiceScope scope = this.ServiceScopeFactory.CreateScope())
                        {
                            if (this.DatabaseService.DatabaseIsInitialized())
                            {
                                ResultCode code = await scope.ServiceProvider.GetRequiredService<ISupervisorWeather>().AddWeatherAsync(zaptoWeather);
                                if (code != ResultCode.Ok)
                                {
                                    Log.Error("Error : No Storage for Weather");
                                }
                            }
                        }
                    }

                    await Task.Delay(this.AcquisitionPeriod, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex.Message);
                }
            }
        }
        #endregion
    }
}

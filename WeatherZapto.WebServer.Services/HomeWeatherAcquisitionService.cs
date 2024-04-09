using Framework.Core.Base;
using Framework.Data.Abstractions;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WeatherZapto.Application;
using WeatherZapto.Data;
using WeatherZapto.Model;

namespace WeatherZapto.WebServer.Services
{
    public sealed class HomeWeatherAcquisitionService : CronScheduledService
    {
        #region Properties
        //Every 10 minutes
        protected override string Schedule => "*/10 * * * *";
        #endregion

        #region Constructor
        public HomeWeatherAcquisitionService(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {

        }
        #endregion

        #region Methods
        public override async Task ProcessInScope(IServiceScope scope)
        {
            try
            {
                IApplicationOWService applicationOWService = scope.ServiceProvider.GetRequiredService<IApplicationOWService>();
                IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                IDatabaseService databaseService = scope.ServiceProvider.GetRequiredService<IDatabaseService>();    

                ZaptoWeather zaptoWeather = await applicationOWService.GetCurrentWeather(configuration["OpenWeatherAPIKey"],
                                                                                                    configuration["HomeLocation"],
                                                                                                    configuration["HomeLongitude"],
                                                                                                    configuration["HomeLatitude"],
                                                                                                    "fr");
                if (zaptoWeather != null)
                {
                    if (databaseService.DatabaseIsInitialized())
                    {
                        ResultCode code = await scope.ServiceProvider.GetRequiredService<ISupervisorWeather>().AddWeatherAsync(zaptoWeather);
                        if (code != ResultCode.Ok)
                        {
                            Log.Error("Error : No Storage for Weather");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }
        }
        #endregion
    }
}

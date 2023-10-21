using Framework.Core.Domain;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using WeatherZapto.Application;
using WeatherZapto.Model;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface IAirPollutionViewModel : IBaseViewModel
    {
        Task<AirPollutionModel?> GetAirPollutionModel(string location, string longitude, string latitude);
        Task<AirPollutionModel?> GetAirPollutionModel(string longitude, string latitude);
        Task<AirPollutionModel?> GetAirPollutionModel();
        void OpenDetails(AirPollutionModel model);
    }

    public class AirPollutionViewModel : BaseViewModel, IAirPollutionViewModel
    {
        #region Properties
        private IApplicationAirPollutionService ApplicationAirPollutionService { get; }
        #endregion

        #region Constructor
        public AirPollutionViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            ApplicationAirPollutionService = serviceProvider.GetRequiredService<IApplicationAirPollutionService>();
        }
        #endregion

        #region Methods
        public override async Task InitializeAsync(string? parameter)
        {
            await base.InitializeAsync(parameter);
        }

        public void OpenDetails(AirPollutionModel model)
        {
            this.NavigationService.NavigateTo($"/airpollutiondetails", model);
        }

        public async Task<AirPollutionModel?> GetAirPollutionModel()
        {
            AirPollutionModel? model = null;
            try
            {
                ZaptoUser user = await AuthenticationService.GetAuthenticatedUser();
                if (user != null)
                {
                    ZaptoAirPollution zaptoAirPollution = await ApplicationAirPollutionService.GetCurrentAirPollution(user.LocationName, user.LocationLongitude, user.LocationLatitude);
                    if (zaptoAirPollution != null)
                    {
                        model = this.GetModel(zaptoAirPollution);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return model;
        }

        public async Task<AirPollutionModel?> GetAirPollutionModel(string location, string longitude, string latitude)
        {
            AirPollutionModel? model = null;
            try
            {
                ZaptoAirPollution zaptoAirPollution = await ApplicationAirPollutionService.GetCurrentAirPollution(location, longitude, latitude);
                if (zaptoAirPollution != null)
                {
                    model = this.GetModel(zaptoAirPollution);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return model;
        }
        public async Task<AirPollutionModel?> GetAirPollutionModel(string longitude, string latitude)
        {
            AirPollutionModel? model = null;
            try
            {
                ZaptoAirPollution zaptoAirPollution = await ApplicationAirPollutionService.GetCurrentAirPollution(longitude, latitude);
                if (zaptoAirPollution != null)
                {
                    model = this.GetModel(zaptoAirPollution);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return model;
        }
        private AirPollutionModel GetModel(ZaptoAirPollution zaptoAirPollution)
        {
            AirPollutionModel model = new AirPollutionModel()
            {
                Id = zaptoAirPollution.Id,
                aqi = zaptoAirPollution.aqi,
                Location = zaptoAirPollution.Location,
                Longitude = zaptoAirPollution.Longitude,
                Latitude = zaptoAirPollution.Latitude,
                TimeStamp = zaptoAirPollution.TimeStamp,
            };

            model.Items?.Add(new AirPollutionItemModel()
            {
                Name = "co",
                Value = zaptoAirPollution.co,
                Levels = new int[5] { 0, 4400, 9400, 12400, 15400 }
            });

            model.Items?.Add(new AirPollutionItemModel()
            {
                Name = "no2",
                Value = zaptoAirPollution.no2,
                Levels = new int[5] { 0, 40, 70, 150, 200 }
            });

            model.Items?.Add(new AirPollutionItemModel()
            {
                Name = "o3",
                Value = zaptoAirPollution.o3,
                Levels = new int[5] { 0, 60, 100, 140, 180 }
            });

            model.Items?.Add(new AirPollutionItemModel()
            {
                Name = "so2",
                Value = zaptoAirPollution.so2,
                Levels = new int[5] { 0, 20, 80, 250, 350 }
            });

            model.Items?.Add(new AirPollutionItemModel()
            {
                Name = "pm10",
                Value = zaptoAirPollution.pm10,
                Levels = new int[5] { 0, 20, 50, 100, 200 }
            });

            model.Items?.Add(new AirPollutionItemModel()
            {
                Name = "pm2_5",
                Value = zaptoAirPollution.pm2_5,
                Levels = new int[5] { 0, 10, 25, 50, 75 }
            });

            model.Items?.Add(new AirPollutionItemModel()
            {
                Name = "nh3",
                Value = zaptoAirPollution.nh3,
            });

            model.Items?.Add(new AirPollutionItemModel()
            {
                Name = "no",
                Value = zaptoAirPollution.no,
            });

            return model;
        }
        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion
    }
}

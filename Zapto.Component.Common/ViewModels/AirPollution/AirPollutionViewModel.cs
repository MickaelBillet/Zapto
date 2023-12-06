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
                    this.IsLoading = true;

                    ZaptoAirPollution zaptoAirPollution = await ApplicationAirPollutionService.GetCurrentAirPollution(user.LocationName, user.LocationLongitude, user.LocationLatitude);
                    if (zaptoAirPollution != null)
                    {
                        model = this.Map(zaptoAirPollution);
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                throw new Exception("AirPollution Service Exception : " + ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
            return model;
        }

        public async Task<AirPollutionModel?> GetAirPollutionModel(string location, string longitude, string latitude)
        {
            AirPollutionModel? model = null;
            try
            {
                this.IsLoading = true;

                ZaptoAirPollution zaptoAirPollution = await ApplicationAirPollutionService.GetCurrentAirPollution(location, longitude, latitude);
                if (zaptoAirPollution != null)
                {
                    model = this.Map(zaptoAirPollution);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw new Exception("AirPollution Service Exception : " + ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
            return model;
        }
        public async Task<AirPollutionModel?> GetAirPollutionModel(string longitude, string latitude)
        {
            AirPollutionModel? model = null;
            try
            {
                this.IsLoading = true;

                ZaptoAirPollution zaptoAirPollution = await ApplicationAirPollutionService.GetCurrentAirPollution(longitude, latitude);
                if (zaptoAirPollution != null)
                {
                    model = this.Map(zaptoAirPollution);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw new Exception("AirPollution Service Exception : " + ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
            return model;
        }
        private AirPollutionModel Map(ZaptoAirPollution zaptoAirPollution)
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

            model.Items?.Add(new AirPollutionItemModel(this.Localizer["CO"], "co",   zaptoAirPollution.co, new int[5] { 0, 4400, 9400, 12400, 15400 }));
            model.Items?.Add(new AirPollutionItemModel(this.Localizer["NO2"], "no2", zaptoAirPollution.no2, new int[5] { 0, 40, 70, 150, 200 }));
            model.Items?.Add(new AirPollutionItemModel(this.Localizer["O3"], "o3", zaptoAirPollution.o3, new int[5] { 0, 60, 100, 140, 180 }));
            model.Items?.Add(new AirPollutionItemModel(this.Localizer["SO2"], "so2", zaptoAirPollution.so2, new int[5] { 0, 20, 80, 250, 350 }));
            model.Items?.Add(new AirPollutionItemModel(this.Localizer["PM10"], "pm10", zaptoAirPollution.pm10, new int[5] { 0, 20, 50, 100, 200 }));
            model.Items?.Add(new AirPollutionItemModel(this.Localizer["PM25"], "pm2_5", zaptoAirPollution.pm2_5, new int[5] { 0, 10, 25, 50, 75 }));
            model.Items?.Add(new AirPollutionItemModel(this.Localizer["NH3"], "nh3", zaptoAirPollution.nh3, null));
            model.Items?.Add(new AirPollutionItemModel(this.Localizer["NO"], "no", zaptoAirPollution.no, null));

            return model;
        }
        public override void Dispose()
        {
            base.Dispose();
        }
        #endregion
    }
}

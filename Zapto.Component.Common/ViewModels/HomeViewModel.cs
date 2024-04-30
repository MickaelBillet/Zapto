using Connect.Application;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WeatherZapto.Application;
using WeatherZapto.Model;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface IHomeViewModel : IBaseViewModel
    {
        Task<string> GetLocation(LocationModel model);
        Task<(LocationModel? model, string location)> GetLocationModel(); 
        Task<string?> GetReverseLocation(string latitude, string longitude);
    }

    public sealed class HomeViewModel : BaseViewModel, IHomeViewModel
    {
        #region Properties
        private IApplicationLocationService ApplicationLocationServices { get; }
        private IApplicationConnectLocationServices ApplicationConnectLocationServices { get; }
        private LocationModel? Model { get; set; }
        private bool LocationFound
        {
            get
            {
                return string.IsNullOrEmpty(this.Model?.Location) == false;
            }
        }

        private bool LocalizationFound
        {
            get
            {
                return (this.Model != null) && (this.Model.Latitude != null) && (this.Model.Longitude != null);
            }
        }

        private bool? LocalizationIsAvailable
        {
            get
            {
                return this.Model?.LocalizationIsAvailable;
            }
        }
        #endregion

        #region Constructor
        public HomeViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.ApplicationLocationServices = serviceProvider.GetRequiredService<IApplicationLocationService>();
            this.ApplicationConnectLocationServices = serviceProvider.GetRequiredService<IApplicationConnectLocationServices>();
        }
        #endregion

        #region Methods
        public async Task<string> GetLocation(LocationModel model)
        {
            string location = string.Empty;
            this.Model = model;

            try
            {
                if (this.Model != null)
                {
                    this.IsLoading = true;

                    if ((this.LocationFound == true) && (this.LocalizationFound == true))
                    {
                        location = this.Model.Location!;
                    }
                    if ((this.LocalizationIsAvailable == null) && (this.LocationFound == false))
                    {
                        location = this.Localizer["Location in progress"];
                    }
                    else if ((this.LocalizationIsAvailable == true) && (this.LocationFound == false) && (this.LocalizationFound == true))
                    {
                        this.Model.Location = await this.GetReverseLocation(this.Model.Latitude.ToString()!, this.Model.Longitude.ToString()!);
                        if (this.Model?.Location != null)
                        {
                            location = this.Model.Location;
                        }
                        else
                        {
                            location = this.Localizer["Unable to locate"];
                        }
                    }
                    else if (this.LocalizationIsAvailable == false)
                    {
                        location = this.Localizer["Unable to locate"];
                    }
                }
            }
            catch (Exception ex)
            {
                location = this.Localizer["Unable to locate"];
                Log.Debug($"{ClassHelper.GetCallerClassAndMethodName()} - {ex.ToString()}");
                this.NavigationService.ShowMessage("Location Unable", ZaptoSeverity.Error);
            }
            finally
            {
                this.IsLoading = false;
            }

            return location;
        }
        public async Task<string?> GetReverseLocation(string latitude, string longitude)
        {
            ZaptoLocation? zaptoLocation = null;
            try
            {
                this.IsLoading = true;
                zaptoLocation = await this.ApplicationLocationServices.GetReverseLocation(longitude, latitude);
            }
            catch (Exception ex)
            {
                Log.Debug($"{ClassHelper.GetCallerClassAndMethodName()} - {ex.ToString()}");
                throw new Exception("Location Service Exception : " + ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
            return zaptoLocation?.Location;
        }
        public async Task<(LocationModel? model, string location)> GetLocationModel()
        {
            string location = string.Empty;
            LocationModel? model = null;

            try
            {
                this.IsLoading = true;

                model = (await this.ApplicationConnectLocationServices.GetLocations())?.Select((location) => new LocationModel()
                {
                    Location = location.City,
                    Id = location.Id
                }).FirstOrDefault();

                if (model?.Location != null)
                {
                    location = model.Location;
                }
                else
                {
                    location = this.Localizer["Unable to locate"];
                }
            }
            catch (Exception ex)
            {
                Log.Debug($"{ClassHelper.GetCallerClassAndMethodName()} - {ex.ToString()}");
                location = this.Localizer["Unable to locate"];
                this.NavigationService.ShowMessage("Location Service Exception", ZaptoSeverity.Error);
            }
            finally
            {
                this.IsLoading = false;
            }
            return (model, location);
        }
        #endregion
    }
}

﻿using Connect.Application;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WeatherZapto.Application;
using WeatherZapto.Model;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface ILocationViewModel : IBaseViewModel
    {
        Task<string?> GetReverseLocation(string latitude, string longitude);
        Task TestNotification(string? locationId);
        Task<LocationModel?> GetLocationModel();
        Task<string> GetLocation(LocationModel model);
    }

    public class LocationViewModel : BaseViewModel, ILocationViewModel
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
        public LocationViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
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

        public async Task TestNotification(string? locationId)
        {
            try
            {
                if (locationId != null)
                {
                    await this.ApplicationConnectLocationServices.TestNotication(locationId);
                }
            }
            catch (Exception ex)
            {
                Log.Debug($"{ClassHelper.GetCallerClassAndMethodName()} - {ex.ToString()}");
            }
        }

        public async Task<LocationModel?> GetLocationModel()
        {
            LocationModel? model = null;
            try
            {
                this.IsLoading = true;

                model = (await this.ApplicationConnectLocationServices.GetLocations())?.Select((location) => new LocationModel()
                {
                    Location = location.City,
                    Id = location.Id
                }).FirstOrDefault();
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
            return model;
        }
        #endregion
    }
}

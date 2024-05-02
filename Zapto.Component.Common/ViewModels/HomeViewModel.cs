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
        Task SetLocation(double longitude, double latitude);
        LocationModel SetLocationModel(LocationModel model);
        Task SetError();
    }

    public sealed class HomeViewModel : BaseViewModel, IHomeViewModel
    {
        #region Properties
        private IApplicationLocationService ApplicationLocationServices { get; }
        private LocationModel? Model { get; set; }
      
        #endregion

        #region Constructor
        public HomeViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.ApplicationLocationServices = serviceProvider.GetRequiredService<IApplicationLocationService>();
        }
        #endregion

        #region Methods

        public override async Task InitializeAsync(object? parameter)
        {            
            this.Model = parameter as LocationModel;
            if (this.Model != null)
            {
                this.Model.LocalizationIsAvailable = ProgressStaus.InProgress;
                this.Model.LocationIsAvailable = ProgressStaus.NoAvailable;
            }
            await Task.CompletedTask;
        }
        public async Task SetLocation(double longitude, double latitude)
        {
            try
            {
                if (this.Model != null)
                {
                    this.Model.LocalizationIsAvailable = ProgressStaus.Available;
                    this.IsLoading = true;

                    if ((this.Model.LocalizationIsAvailable == ProgressStaus.Available) && (this.Model.LocationIsAvailable == ProgressStaus.NoAvailable))
                    {
                        this.Model.Location = await this.GetReverseLocation(latitude.ToString()!, longitude.ToString()!);
                        if (this.Model != null)
                        {
                            if (this.Model?.Location != null)
                            {
                                this.Model.Latitude = latitude;
                                this.Model.Longitude = longitude;
                                this.Model.LocationIsAvailable = ProgressStaus.Available;
                            }
                            else
                            {
                                this.Model!.LocationIsAvailable = ProgressStaus.NoAvailable;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (this.Model != null)
                {
                    this.Model.LocationIsAvailable = ProgressStaus.NoAvailable;
                }
                Log.Debug($"{ClassHelper.GetCallerClassAndMethodName()} - {ex.ToString()}");
                this.NavigationService.ShowMessage("Location Unable", ZaptoSeverity.Error);
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        public LocationModel SetLocationModel(LocationModel model)
        {
            model.LocalizationIsAvailable = ProgressStaus.Available;
            model.LocationIsAvailable = (string.IsNullOrEmpty(model.Location) == false) ? ProgressStaus.Available : ProgressStaus.NoAvailable;
            return model;
        }

        public async Task SetError()
        {
            if (this.Model != null)
            {
                this.Model.LocalizationIsAvailable = await Task.FromResult<byte>(ProgressStaus.NoAvailable);
            }
        }

        private async Task<string?> GetReverseLocation(string latitude, string longitude)
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
        #endregion
    }
}

using Connect.Application;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface IDashboardViewModel : IBaseViewModel
	{
        Task<LocationModel> GetLocationModel();
    }

    public sealed class DashboardViewModel : BaseViewModel, IDashboardViewModel
	{
        #region Properties
        private IApplicationConnectLocationServices ApplicationConnectLocationServices { get; }
        public byte LocalizationIsAvailable { get; set; } = ProgressStaus.None;
        public byte LocationIsAvailable { get; set; } = ProgressStaus.None;
        #endregion

        #region Constructor
        public DashboardViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
            this.ApplicationConnectLocationServices = serviceProvider.GetRequiredService<IApplicationConnectLocationServices>();
        }
        #endregion

        #region Methods
        public async Task<LocationModel> GetLocationModel()
        {
            LocationModel? model = new LocationModel()
            {
                LocalizationIsAvailable = ProgressStaus.InProgress,
                LocationIsAvailable = ProgressStaus.NoAvailable
            };

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
                    model.LocationIsAvailable = ProgressStaus.Available;
                }
                else
                {
                    model = new LocationModel()
                    {
                        LocationIsAvailable = ProgressStaus.NoAvailable
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Debug($"{ClassHelper.GetCallerClassAndMethodName()} - {ex.ToString()}");
                this.NavigationService.ShowMessage("Location Service Exception", ZaptoSeverity.Error);
                model = new LocationModel()
                {
                    LocationIsAvailable = ProgressStaus.NoAvailable
                };
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

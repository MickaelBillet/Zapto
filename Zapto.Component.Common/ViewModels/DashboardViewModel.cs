using Connect.Application;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface IDashboardViewModel : IBaseViewModel
	{
        Task<LocationModel?> GetLocationModel();
    }

    public sealed class DashboardViewModel : BaseViewModel, IDashboardViewModel
	{
        #region Properties
        private IApplicationConnectLocationServices ApplicationConnectLocationServices { get; }
        #endregion

        #region Constructor
        public DashboardViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
            this.ApplicationConnectLocationServices = serviceProvider.GetRequiredService<IApplicationConnectLocationServices>();
        }
        #endregion

        #region Methods
        public async Task<LocationModel?> GetLocationModel()
        {
            LocationModel? model = null;
            try
            {
                model = (await this.ApplicationConnectLocationServices.GetLocations())?.Select((location) => new LocationModel()
                {
                    Name = location.City,
                    Id = location.Id
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw new Exception("Location Service Exception : " + ex.Message);
            }
            return model;
        }
        #endregion
    }
}

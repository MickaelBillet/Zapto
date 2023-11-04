using Connect.Application;
using Microsoft.Extensions.DependencyInjection;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface IDashboardViewModel : IBaseViewModel
	{
		Task<LocationModel?> GetLocationModel();
		Task TestNotification(string? locationId);
    }

	public sealed class DashboardViewModel : BaseViewModel, IDashboardViewModel
	{
		#region Properties
		private ILocationViewModel	LocationViewModel { get; }
        private IApplicationLocationServices ApplicationLocationServices { get; }
        #endregion

        #region Constructor
        public DashboardViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.LocationViewModel = serviceProvider.GetRequiredService<ILocationViewModel>();
			this.ApplicationLocationServices = serviceProvider.GetRequiredService<IApplicationLocationServices>();
		}
		#endregion

		#region Methods
		public override async Task InitializeAsync(string? parameter)
		{
			await base.InitializeAsync(parameter);
		}

		public async Task<LocationModel?> GetLocationModel()
		{
			return await this.LocationViewModel.GetLocationModel();
		}

		public async Task TestNotification(string? locationId)
		{
			if (locationId != null)
			{
				await this.ApplicationLocationServices.TestNotication(locationId);
			}
		}
        #endregion
    }
}

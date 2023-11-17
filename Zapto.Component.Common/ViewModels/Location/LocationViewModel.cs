using Connect.Application;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface ILocationViewModel : IBaseViewModel
    {
		Task<LocationModel?> GetLocationModel();
	}

	public class LocationViewModel : BaseViewModel, ILocationViewModel
	{
		#region Properties
		private IApplicationLocationServices ApplicationLocationServices { get; }

		#endregion

		#region Constructor
		public LocationViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.ApplicationLocationServices = serviceProvider.GetRequiredService<IApplicationLocationServices>();
		}
		#endregion

		#region Methods
		public override async Task InitializeAsync(string? parameter)
		{
			await base.InitializeAsync(parameter);
		}
		public override void Dispose()
		{
			base.Dispose();
		}
		public async Task<LocationModel?> GetLocationModel()
		{
			LocationModel? model = null;
			try
			{
				model = (await this.ApplicationLocationServices.GetLocations())?.Select((location) => new LocationModel()
				{
					Name = location.City,
					Id = location.Id
				}).FirstOrDefault();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				throw ex;
			}
			return model;
		}
		#endregion
	}
}

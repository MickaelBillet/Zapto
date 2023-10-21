using Microsoft.Extensions.DependencyInjection;
using WeatherZapto.Application;
using WeatherZapto.Model;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface IHomeViewModel : IBaseViewModel
    {
        Task<LocationModel?> GetLocationModel(string latitude, string longitude);
    }

    public class HomeViewModel : BaseViewModel, IHomeViewModel
    {
        #region Properties
        private IApplicationLocationService ApplicationLocationService { get; }
        #endregion

        #region Constructor
        public HomeViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.ApplicationLocationService = serviceProvider.GetRequiredService<IApplicationLocationService>();
        }
        #endregion

        #region Methods
        public override async Task InitializeAsync(string? parameter)
        {
            await base.InitializeAsync(parameter);
        }
        public async Task<LocationModel?> GetLocationModel(string latitude, string longitude)
        {
            ZaptoLocation location = await this.ApplicationLocationService.GetLocation(longitude, latitude);
            return (location != null) ? new LocationModel() { Name = location.Location } : null;
        }
        #endregion
    }
}

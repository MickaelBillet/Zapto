using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Zapto.Component.Common.Services;

namespace Zapto.Component.Common.ViewModels
{
    public interface IMainViewModel : IBaseViewModel
	{

	}

	public sealed class MainViewModel : BaseViewModel, IMainViewModel
	{
		#region Properties
        private IZaptoLocalStorageService StorageService { get; }
		#endregion

		#region Constructor
		public MainViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
            this.StorageService = serviceProvider.GetRequiredService<IZaptoLocalStorageService>();
		}
        #endregion

        #region  Methods
        public override async Task InitializeAsync(object? parameter)
        {
            const string defaultCulture = "en-US";

            var result = await this.StorageService.GetItemAsync<string>("culture");
            var culture = CultureInfo.GetCultureInfo(result ?? defaultCulture);

            if (result == null)
            {
                await this.StorageService.SetItemAsync<string>("culture", defaultCulture);
            }

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
        #endregion
    }
}

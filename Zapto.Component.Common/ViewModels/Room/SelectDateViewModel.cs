using Microsoft.Extensions.DependencyInjection;
using Zapto.Component.Common.Services;

namespace Zapto.Component.Common.ViewModels
{
    public interface ISelectDateViewModel : IBaseViewModel
    {
        Task SetDateFromLocalStorage<T>(string key, T value);
    }

    public class SelectDateViewModel : BaseViewModel, ISelectDateViewModel
    {
        #region Properties
        private IZaptoLocalStorageService ZaptoLocalStorageService { get; }
        #endregion

        #region Constructor
        public SelectDateViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.ZaptoLocalStorageService = serviceProvider.GetRequiredService<IZaptoLocalStorageService>();
        }
        #endregion

        #region Methods
        public async Task SetDateFromLocalStorage<T>(string key, T value)
        {
            await this.ZaptoLocalStorageService.SetItemAsync<T>(key, value);
        }
        #endregion
    }
}

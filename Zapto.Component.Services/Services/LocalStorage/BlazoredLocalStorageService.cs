using Microsoft.Extensions.DependencyInjection;
using Zapto.Component.Common.IServices;
using ILocalStorageService = Blazored.LocalStorage.ILocalStorageService;

namespace Zapto.Component.Services
{
    public class BlazoredLocalStorageService : IZaptoLocalStorageService
    {
        #region Properties
        private ILocalStorageService? LocalStorageService { get; }
        #endregion

        #region Constructor
        public BlazoredLocalStorageService(IServiceProvider serviceProvider)
        {
            this.LocalStorageService = serviceProvider.GetRequiredService<ILocalStorageService>();
        }
        #endregion

        #region Methods
        public async Task<T?> GetItemAsync<T>(string key) where T : class
        {
            return (this.LocalStorageService != null) ? (await this.LocalStorageService.GetItemAsync<T>(key)) : null;
        }

        public async Task SetItemAsync<T>(string key, T value)
        {
            if (this.LocalStorageService != null)
            {
                await this.LocalStorageService.SetItemAsync<T>(key, value);
            }
        }
        #endregion
    }
}

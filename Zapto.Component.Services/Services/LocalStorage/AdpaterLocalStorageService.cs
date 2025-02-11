using Microsoft.JSInterop;
using Zapto.Component.Common.Services;

namespace Zapto.Component.Services
{
    public class AdpaterLocalStorageService : IZaptoStorageService
    {
        #region Service
        private ILocalStorageService StorageService { get; }
        private IJSRuntime JSRuntime { get; }
        #endregion

        #region Constructor
        public AdpaterLocalStorageService(ILocalStorageService storageService, IJSRuntime runtime)
        {
            this.StorageService = storageService;
            this.JSRuntime = runtime;
        }
        #endregion

        #region Methods
        public Task<T?> GetItemAsync<T>(string key) where T : class
        {
            return this.StorageService.GetItemAsync<T>(this.JSRuntime, key);
        }
        public async Task SetItemAsync<T>(string key, T value)
        {
            await this.StorageService.SetItemAsync<T>(this.JSRuntime, key, value); 
        }
        #endregion
    }
}

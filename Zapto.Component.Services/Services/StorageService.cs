using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Zapto.Component.Common.IServices;

namespace Zapto.Component.Services
{
    public class StorageService : IStorageService
    {
        #region Properties
        private IJSRuntime JSRuntime { get; set; }
        #endregion

        #region Constructor
        public StorageService(IServiceProvider serviceProvider) 
        {
            this.JSRuntime = serviceProvider.GetRequiredService<IJSRuntime>();
        }
        #endregion

        #region Methods
        public async Task<T> GetItemAsync<T>(string key) 
        {
            return await this.JSRuntime.InvokeAsync<T>("localStorage.getItem", key);
        }

        public async Task SetItemAsync<T>(string key, T value)
        {
            await this.JSRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
        }
        #endregion
    }
}

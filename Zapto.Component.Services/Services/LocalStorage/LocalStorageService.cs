using Microsoft.JSInterop;
using Zapto.Component.Common.Services;

namespace Zapto.Component.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        #region Constructor
        public LocalStorageService(IServiceProvider serviceProvider) 
        {
        }
        #endregion

        #region Methods
        public async Task<T?> GetItemAsync<T>(IJSRuntime jSRuntime, string key) where T : class
        {
            return await jSRuntime.InvokeAsync<T>($"{key}.get");
        }

        public async Task SetItemAsync<T>(IJSRuntime jSRuntime, string key, T value)
        {
            await jSRuntime.InvokeVoidAsync($"{key}.set", value);
        }
        #endregion
    }
}

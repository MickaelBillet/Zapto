using Microsoft.JSInterop;
using Zapto.Component.Common.IServices;
using Zapto.Component.Services.JsInterop;

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
        public async Task<T?> GetItemAsync<T>(IJSRuntime jsRuntime, string key) 
        {
            await using (LocalStorageAccessorJsInterop jsInterop = new LocalStorageAccessorJsInterop(jsRuntime))
            {
                return await jsInterop.GetValueAsync<T>(key);
            }
        }

        public async Task SetItemAsync<T>(IJSRuntime jsRuntime, string key, T value)
        {
            await using (LocalStorageAccessorJsInterop jsInterop = new LocalStorageAccessorJsInterop(jsRuntime))
            {
                await jsInterop.SetValueAsync<T>(key, value);
            }
        }
        #endregion
    }
}

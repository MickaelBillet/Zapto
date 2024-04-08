using Microsoft.JSInterop;

namespace Zapto.Component.Common.IServices
{
    public interface ILocalStorageService
    {
        Task<T?> GetItemAsync<T>(IJSRuntime jsRuntime, string key);
        Task SetItemAsync<T>(IJSRuntime jsRuntime, string key, T value);
    }
}

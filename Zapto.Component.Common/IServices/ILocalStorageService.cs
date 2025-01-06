using Microsoft.JSInterop;

namespace Zapto.Component.Common.Services
{
    public interface ILocalStorageService
    {
        Task<T?> GetItemAsync<T>(IJSRuntime jSRuntime, string key) where T : class;
        Task SetItemAsync<T>(IJSRuntime jSRuntime, string key, T value);
    }
}

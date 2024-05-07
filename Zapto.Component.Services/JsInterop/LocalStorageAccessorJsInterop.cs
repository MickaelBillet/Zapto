using Microsoft.JSInterop;

namespace Zapto.Component.Services.JsInterop
{
    public class LocalStorageAccessorJsInterop : IAsyncDisposable
    {
        private Lazy<IJSObjectReference> moduleTask = new();
        private readonly IJSRuntime jsRuntime;

        #region Constructor
        public LocalStorageAccessorJsInterop(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }
        #endregion

        #region Methods
        private async Task WaitForReference()
        {
            if (moduleTask.IsValueCreated is false)
            {
                moduleTask = new(await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./scripts/storage/LocalStorageAccessor.js"));
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                await moduleTask.Value.DisposeAsync();
            }
        }
        public async Task<T> GetValueAsync<T>(string key)
        {
            await WaitForReference();
            var result = await moduleTask.Value.InvokeAsync<T>("get", key);

            return result;
        }

        public async Task SetValueAsync<T>(string key, T value)
        {
            await WaitForReference();
            await moduleTask.Value.InvokeVoidAsync("set", key, value);
        }

        public async Task Clear()
        {
            await WaitForReference();
            await moduleTask.Value.InvokeVoidAsync("clear");
        }

        public async Task RemoveAsync(string key)
        {
            await WaitForReference();
            await moduleTask.Value.InvokeVoidAsync("remove", key);
        }
        #endregion
    }
}

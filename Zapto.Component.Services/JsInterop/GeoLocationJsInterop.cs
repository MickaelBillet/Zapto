using Microsoft.JSInterop;

namespace Zapto.Component.Common.JsInterop
{
    public class GeoLocationJsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;
        private readonly DotNetObjectReference<GeoLocationJsInterop> dotNetObjectReference;
        private Func<double, double, Task>? CallBackSuccessResult { get; set; }
        private Func<string, Task>? CallBackErrorResult = default!;

        #region Constructor
        public GeoLocationJsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./scripts/geoLocations/geoLocationJsInterop.js").AsTask());

            dotNetObjectReference = DotNetObjectReference.Create(this);
        }
        #endregion

        #region Methods
        public async ValueTask GetLocationAsync(Func<double, double, Task>? callbackSuccess, Func<string, Task>? callbackError)
        {
            this.CallBackSuccessResult = callbackSuccess;
            this.CallBackErrorResult = callbackError;
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("getCurrentPosition", dotNetObjectReference);
        }

        [JSInvokable]
        public async Task OnSuccessAsync(GeoCoordinates geoCoordinates)
        {
            if (this.CallBackSuccessResult != null)
            {
                await this.CallBackSuccessResult(geoCoordinates.Longitude, geoCoordinates.Latitude);
            }
        }

        [JSInvokable]
        public async Task OnErrorAsync(string error)
        {
            if (this.CallBackErrorResult != null)
            {
                await this.CallBackErrorResult.Invoke(error);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
        #endregion
    }
    public class GeoCoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Accuracy { get; set; }
    }
}

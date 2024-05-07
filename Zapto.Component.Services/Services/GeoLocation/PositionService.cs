using Microsoft.JSInterop;
using Zapto.Component.Common.JsInterop;
using Zapto.Component.Common.Services;

namespace Zapto.Component.Services
{
    public class PositionService : IPositionService
    {
        #region Constructor
        public PositionService(IServiceProvider serviceProvider)
        {

        }
        #endregion

        #region Methods
        public async Task GetCurrentPosition(IJSRuntime jsRuntime, Func<double, double, Task>? callbackSuccess, Func<string, Task>? callbackError)
        {
            await using (GeoLocationJsInterop geoLocationJsInterop = new GeoLocationJsInterop(jsRuntime))
            {
                await geoLocationJsInterop.GetLocationAsync(callbackSuccess, callbackError);
            }
        }
        #endregion
    }
}

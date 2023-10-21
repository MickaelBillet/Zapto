using Microsoft.JSInterop;

namespace Zapto.Component.Common.Services
{
	public interface IPositionService
    {
        Task GetCurrentPosition(IJSRuntime jsRuntime, Func<double, double, Task>? callbackSuccess, Func<string, Task>? callbackError = null);
    }
}

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;
using Zapto.Component.Common.Services;

namespace Zapto.Web.Dashboard.Configuration
{
    public static class WebAssemblyHostExtension
    {
        public async static Task SetDefaultUICulture(this WebAssemblyHost host)
        {
            const string defaultCulture = "en-US";
            var storageService = host.Services.GetRequiredService<IZaptoStorageService>();
            const int maxRetries = 3;
            string? result = null;

            for (int i = 0; i < maxRetries; i++)
            {
                result = await storageService.GetItemAsync<string>("blazorCulture");
                if (result != null)
                {
                    break;
                }
                await Task.Delay(100);
            }

            if (result == null)
            {
                await storageService.SetItemAsync("blazorCulture", defaultCulture);
                result = await storageService.GetItemAsync<string>("blazorCulture");
            }

            var culture = CultureInfo.GetCultureInfo(result ?? defaultCulture);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}

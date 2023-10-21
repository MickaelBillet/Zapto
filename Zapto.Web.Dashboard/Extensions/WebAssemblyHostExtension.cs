using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;

namespace Zapto.Web.Dashboard.Extensions
{
	public static class WebAssemblyHostExtension
    {
        public async static Task SetDefaultCulture(this WebAssemblyHost host)
        {
            var localStorage = host.Services.GetRequiredService<ILocalStorageService>();
            var result = await localStorage.GetItemAsync<string>("culture");

            CultureInfo culture;
            if (result != null)
            {
                culture = new CultureInfo(result);
            }
            else
            {
                culture = new CultureInfo("fr-FR");
                await localStorage.SetItemAsync<string>("culture", culture.Name);
            }

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}

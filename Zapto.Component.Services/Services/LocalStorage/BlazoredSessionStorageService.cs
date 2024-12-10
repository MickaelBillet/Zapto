using Microsoft.Extensions.DependencyInjection;
using Zapto.Component.Common.Services;
using ISessionStorageService = Blazored.SessionStorage.ISessionStorageService;

namespace Zapto.Component.Services
{
    public class BlazoredSessionStorageService : IZaptoStorageService
    {
        #region Properties
        private ISessionStorageService? SessionStorageService { get; }
        #endregion

        #region Constructor
        public BlazoredSessionStorageService(IServiceProvider serviceProvider)
        {
            this.SessionStorageService = serviceProvider.GetRequiredService<ISessionStorageService>();
        }
        #endregion

        #region Methods
        public async Task<T?> GetItemAsync<T>(string key) where T : class
        {
            return (this.SessionStorageService != null) ? (await this.SessionStorageService.GetItemAsync<T>(key)) : null;
        }

        public async Task SetItemAsync<T>(string key, T value)
        {
            if (this.SessionStorageService != null)
            {
                await this.SessionStorageService.SetItemAsync<T>(key, value);
            }
        }
        #endregion
    }
}

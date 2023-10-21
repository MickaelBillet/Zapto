using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Zapto.Component.Common.Services;

namespace Zapto.Component.Services
{
    public class NavigationService : INavigationService
	{
		#region Service
		private NavigationManager NavigationManager { get; }
		private DataService DataService { get; }
		#endregion

		#region Constructor
		public NavigationService(IServiceProvider serviceProvider)
		{
			this.NavigationManager = serviceProvider.GetRequiredService<NavigationManager>();
			this.DataService = serviceProvider.GetRequiredService<DataService>();
		}
		#endregion

		#region Methods
		public void NavigateTo(string path, bool forceReload = false)
		{
			this.NavigationManager.NavigateTo(path, forceReload);
		}

        public void NavigateTo(string path, Object data, bool forceReload = false)
        {
			this.DataService.Data = data;
            this.NavigationManager.NavigateTo(path, forceReload);
        }

        public void NavigateToLogout(string path, string? returnurl = null)
        {
            this.NavigationManager.NavigateToLogout(path, returnurl);
        }

        public string GetUri()
		{
			return this.NavigationManager.Uri;
		}

		public string GetBaseUri()
		{
			return this.NavigationManager.BaseUri;
		}

		public void Dispose()
		{
		}
		#endregion
	}
}

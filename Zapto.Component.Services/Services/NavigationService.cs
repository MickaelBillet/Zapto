using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using Zapto.Component.Common.Services;

namespace Zapto.Component.Services
{
    public class NavigationService : INavigationService
	{
		#region Service
		private NavigationManager NavigationManager { get; }
		private DataService DataService { get; }
		private ISnackbar Snackbar { get; }
		#endregion

		#region Constructor
		public NavigationService(IServiceProvider serviceProvider)
		{
			this.NavigationManager = serviceProvider.GetRequiredService<NavigationManager>();
			this.DataService = serviceProvider.GetRequiredService<DataService>();
			this.Snackbar = serviceProvider.GetRequiredService<ISnackbar>();
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

		public void NavigateToLogin(string path)
		{
            this.NavigationManager.NavigateToLogin(path);
        }

        public string GetUri()
		{
			return this.NavigationManager.Uri;
		}

		public string GetBaseUri()
		{
			return this.NavigationManager.BaseUri;
		}

        public void ShowMessage(string message, byte severity)
		{
			this.Snackbar.Add(message, (Severity)severity, config =>
			{
                config.VisibleStateDuration = 10000;
            });
		}

        public void Dispose()
		{
		}
		#endregion
	}
}

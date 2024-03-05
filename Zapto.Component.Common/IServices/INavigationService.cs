namespace Zapto.Component.Common.Services
{
    public interface INavigationService : IDisposable
	{
		void NavigateTo(string path, Object data, bool forceReload = false);
        void NavigateTo(string path, bool forceReload = false);
		public void NavigateToLogout(string path, string? returnurl = null);
        string GetUri();
		string GetBaseUri();
		void ShowMessage(string message, byte severity);
	}
}

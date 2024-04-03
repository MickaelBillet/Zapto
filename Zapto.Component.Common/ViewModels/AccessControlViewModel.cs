namespace Zapto.Component.Common.ViewModels
{
    public interface IAccessControlViewModel : IBaseViewModel
	{
		public void SignOut();
		public void SignIn();
	}

	public sealed class AccessControlViewModel : BaseViewModel, IAccessControlViewModel
	{
		#region Properties
		#endregion

		#region Constructor
		public AccessControlViewModel(IServiceProvider serviceProvider) : base (serviceProvider)
		{
		}
		#endregion

		#region Methods
		public void SignOut()
		{
			this.NavigationService.NavigateToLogout("authentication/logout");
		}
		public void SignIn()
		{
			this.NavigationService.NavigateToLogin("authentication/login");
		}
		#endregion
	}
}

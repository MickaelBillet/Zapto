using AirZapto.Mobile;
using AirZapto.ViewModel;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace AirZapto.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RestartPopupView : PopupPage
	{
		#region Property

		private RestartPopupViewModel ViewModel { get; }

		#endregion

		#region Constructor

		public RestartPopupView()
		{
			InitializeComponent();

			this.BindingContext = this.ViewModel = Host.Current.GetService<RestartPopupViewModel>();
		}

		#endregion
	}
}
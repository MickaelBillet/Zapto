using AirZapto.Mobile;
using AirZapto.ViewModel;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace AirZapto.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CalibrationPopupView : PopupPage
	{
		#region Property

		private CalibrationPopupViewModel ViewModel { get; }

		#endregion

		#region Constructor

		public CalibrationPopupView()
		{
			InitializeComponent();

			this.BindingContext = this.ViewModel = Host.Current.GetService<CalibrationPopupViewModel>();
		}

		#endregion
	}
}
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AirZapto.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPopupView : ContentView
	{
		#region Property

		#endregion

		#region Constructor

		public MenuPopupView()
		{
			InitializeComponent();
		}

		#endregion

		public double FirstSectionHeight
		{
			get
			{
				return FirstSection.Height;
			}
		}
	}
}
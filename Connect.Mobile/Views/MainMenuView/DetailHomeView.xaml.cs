using Connect.Mobile.ViewModel;
using Xamarin.Forms;

namespace Connect.Mobile.View
{
	public partial class DetailHomeView: ContentPage
    {
        private DetailHomeViewModel ViewModel { get; }

        public DetailHomeView()
        {
            InitializeComponent();

            this.BindingContext = this.ViewModel = Host.Current.GetService<DetailHomeViewModel>();
        }
    }
}
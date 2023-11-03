using Connect.Mobile.View.Controls;
using Connect.Mobile.ViewModel;
using Xamarin.Forms.Xaml;

namespace Connect.Mobile.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailView: BasePage
    {
        private DetailViewModel ViewModel { get; }

        public DetailView()
        {
            InitializeComponent();

            this.BindingContext = this.ViewModel = Host.Current.GetService<DetailViewModel>();
        }
    }
}
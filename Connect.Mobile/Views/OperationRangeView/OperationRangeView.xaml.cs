using Connect.Mobile.View.Controls;
using Connect.Mobile.ViewModel;
using Xamarin.Forms.Xaml;

namespace Connect.Mobile.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OperationRangeView : BasePage
    {
        private OperationRangeViewModel ViewModel { get; }

        public OperationRangeView()
        {
            InitializeComponent();

            this.BindingContext = this.ViewModel = Host.Current.GetService<OperationRangeViewModel>();
        }
    }
}
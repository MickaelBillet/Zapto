using Connect.Mobile.View.Controls;
using Xamarin.Forms;

namespace Connect.Mobile.View
{
    public partial class CustomNavigationView : NavigationPage
    {

        public static readonly BindableProperty TransitionTypeProperty = BindableProperty.Create("TransitionType", typeof(TransitionType), typeof(CustomNavigationView), TransitionType.SlideFromLeft);

        public TransitionType TransitionType
        {
            get { return (TransitionType)GetValue(TransitionTypeProperty); }
            set { SetValue(TransitionTypeProperty, value); }
        }

        public CustomNavigationView() : base()
        {
            InitializeComponent();
        }

        public CustomNavigationView(Page root) : base(root)
        {
            InitializeComponent();
        }
    }
}
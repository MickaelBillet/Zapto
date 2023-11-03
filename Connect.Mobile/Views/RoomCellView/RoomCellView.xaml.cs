using Connect.Mobile.ViewModel;
using Xamarin.Forms;

namespace Connect.Mobile.View
{
	public partial class RoomCellView : ContentView
    {
        #region Property

        public static readonly BindableProperty MainMenuHomeViewModelProperty = BindableProperty.Create("MainMenuHomeViewModel", typeof(DetailHomeViewModel), typeof(RoomCellView), null);

        public DetailHomeViewModel MainMenuHomeViewModel
        {
            get { return (DetailHomeViewModel)GetValue(MainMenuHomeViewModelProperty); }
            set { SetValue(MainMenuHomeViewModelProperty, value); }
        }

        #endregion

        #region Constructor

        public RoomCellView()
        {
            InitializeComponent();

            this.MainMenuHomeViewModel = Host.Current.GetService<DetailHomeViewModel>();
        }

        #endregion
    }
}

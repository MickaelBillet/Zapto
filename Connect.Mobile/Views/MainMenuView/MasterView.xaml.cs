using Connect.Mobile.View.Controls;
using Connect.Mobile.ViewModel;
using Xamarin.Forms;

namespace Connect.Mobile.View
{
	public partial class MasterView : BasePage
    {
        #region Property 

        private MasterViewModel ViewModel { get; }

        public ListView ListView { get; set; }

        #endregion

        #region Constructor

        public MasterView()
        {
            InitializeComponent();

            this.BindingContext = this.ViewModel = Host.Current.GetService<MasterViewModel>();

            this.ListView = this.MenuItemsListView;
        }

        #endregion

        #region Method

        #endregion
    }
}
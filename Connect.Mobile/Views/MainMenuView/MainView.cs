using System;
using System.Diagnostics;
using Connect.Mobile.ViewModel;
using Xamarin.Forms;

namespace Connect.Mobile.View
{
	public class MainView : MasterDetailPage
    {
        #region Properties

        private MainViewModel ViewModel { get; }

        #endregion

        #region Constructor

        public MainView()
        {
            this.MasterBehavior = MasterBehavior.SplitOnLandscape;

            this.Master = new MasterView();

            this.Detail = new CustomNavigationView(new DetailHomeView());

            this.BindingContext = this.ViewModel = Host.Current.GetService<MainViewModel>();

            try
            {
                this.IsPresented = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion

        #region Method

        #endregion
    }
}


using System;
using Xamarin.Forms;

namespace AirZapto
{
	public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

        }

        private void MenuItem_Clicked(object sender, EventArgs e)
        {
            App.Current.UserAppTheme = (App.Current.UserAppTheme == OSAppTheme.Light)
                ? OSAppTheme.Dark
                : OSAppTheme.Light;
        }
    }

    public class HiddenItem : ShellItem
    {

    }
}

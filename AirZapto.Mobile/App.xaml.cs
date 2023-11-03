using AirZapto.Mobile;
using AirZapto.Mobile.Resources;
using AirZapto.ViewModel;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AirZapto
{
    public partial class App : Xamarin.Forms.Application
    {
		#region Properties

		public static ResourceManager ResourcesFiles { get; set; }

		public static CultureInfo CurrentCulture { get; set; }

		#endregion

		#region Constructor
		public App()
		{
			InitializeComponent();

            AppResources.Culture = CultureInfo.InstalledUICulture;
		}
		#endregion

		#region Methods
		protected override void OnStart()
		{
            Debug.WriteLine("OnStart");

            try
            {
                this.Initialize();

                this.Launch();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

		protected override void OnSleep()
		{
            Debug.WriteLine("OnSleep");
        }

        protected override void OnResume()
		{
		}

        private void Initialize()
        {
            try
            {
 
            }
            catch (Exception ex)
            {
                Debug.WriteLine((ex));
            }
        }

        private async void Launch()
        {
            try
            {
                this.MainPage = new AppShell();

                App.Current.UserAppTheme = OSAppTheme.Light;

                MainViewModel viewModel = Host.Current.GetService<MainViewModel>();

                await viewModel.Initialize(null).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// This Function is assigned to when the hardware button needs to be handled in code
        /// by the Views or ViewModels. It should mirror what the back/cancel button does in
        /// iOS.
        /// </summary>
        /// <returns>
        /// Three state (nullable) boolean:
        ///  - true  => Complete navigation as normal
        ///  - false => Do not navigate
        ///  - null  => Exit application (Used on screens that limit access to the app)
        /// </returns>
        public static Func<Task<bool?>> HardwareBackPressed
        {
            private get;
            set;
        }

        /// <summary>
        /// This function is used at the platform level when a hardware back button is pressed.
        /// On occasion we want to prevent/confirm backwards navigation or have the views/viewmodels
        /// perform an action on exit. Since iOS handles this without a hardware button, iOS will not
        /// use this method, but Android and WinPhone may both have hardware buttons that need to be
        /// handled.
        /// </summary>
        /// <returns>
        /// Three state (nullable) boolean:
        ///  - true  => Complete navigation as normal
        ///  - false => Do not navigate
        ///  - null  => Exit application (Used primarily on SignInScreen)
        /// </returns>
        public static async Task<bool?> CallHardwareBackPressed()
        {
            Func<Task<bool?>> backPressed = HardwareBackPressed;
            if (backPressed != null)
            {
                return await backPressed();
            }

            return false;
        }

        #endregion
    }
}

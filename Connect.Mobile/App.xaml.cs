using Connect.Mobile.Interfaces;
using Connect.Mobile.Resources;
using Connect.Mobile.ViewModel;
using Framework.Infrastructure.Services;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Xaml.XamlCompilation(Xamarin.Forms.Xaml.XamlCompilationOptions.Compile)]
namespace Connect.Mobile
{
    public partial class App : Xamarin.Forms.Application
    {
        #region Property
        public static ResourceManager ResourcesFiles { get; set; }
        public static CultureInfo CurrentCulture { get; set; }
        public static string LocationId { get; set; }
        #endregion

        #region Constructor
        public App()
        {
            InitializeComponent();
        }
        #endregion

        #region Method

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
            Debug.WriteLine("OnResume");
        }

        private void Initialize()
        {
            try
            {
                if ((Device.RuntimePlatform == Device.iOS) || (Device.RuntimePlatform == Device.Android))
                {
                    CultureInfo ci = Host.Current.GetService<ILocalize>().GetCurrentCultureInfo();
                    AppResources.Culture = ci; // set the RESX for resource localization
                    Host.Current.GetService<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
                }

                Akavache.Registrations.Start("Connect.Mobile");
                Akavache.Sqlite3.Registrations.Start("Connect.Mobile", () => SQLitePCL.Batteries_V2.Init());                
            }
            catch (Exception ex)
            {
                Debug.WriteLine((ex));
            }
        }

        private void Launch()
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                { 
                    await Host.Current.GetService<INavigationService>().NavigateToAsync<MainViewModel>(null);
                    bool res = Host.Current.GetService<IThreadSynchronizationService>().WaitOne("IFirebaseMobileService");
                    Host.Current.GetService<IFirebaseMobileService>().Subscribe(App.LocationId);

                });


                //Must be call in App.xaml.cs
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

using AirZapto.Droid.Services;
using AirZapto.Mobile.Interfaces;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace AirZapto.Droid
{
    [Activity(Label = "AirZapto", Icon = "@drawable/SplashScreen", Theme = "@style/myTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Mobile.Host.Current.Initialize(this.ConfigureServices);
            Rg.Plugins.Popup.Popup.Init(this);

            Forms.SetFlags("Brush_Experimental");
            Forms.SetFlags("Shapes_Experimental");

            Forms.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            try
            {
                LoadApplication(new App());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            services.AddSingleton<IFileHelper, FileHelper>();
            services.AddSingleton<INotification, NotificationHelper>();
        }
    }
}
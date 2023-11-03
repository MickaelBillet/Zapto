
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Connect.Mobile.Interfaces;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Connect.Mobile.Droid
{
    [Activity(Label = "Connect", Icon = "@drawable/C", Theme = "@style/MyTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        internal static readonly string CHANNEL_ID = "Firebase_Cloud_Messaging_Channel";
        internal static readonly int NOTIFICATION_ID = 100;

        public static Context Context;

        public MainActivity()
        {
            Context = this;
        }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);


            Forms.SetFlags("SwipeView_Experimental");

            Forms.Init(this, bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);

            try
            {
                LoadApplication(new App());
                Host.Current.GetRequiredService<IFirebaseMobileService>().LinkToActivity(this, Intent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            Host.Current.GetRequiredService<IFirebaseMobileService>().LinkToActivity(this, intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }        

        public async override void OnBackPressed()
        {
            bool? result = await App.CallHardwareBackPressed();
            if (result == true)
            {
                base.OnBackPressed();
            }
            else if (result == null)
            {
                Finish();
            }
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification 
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID, "FCM Notifications", NotificationImportance.Default)
            {
                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
    }
}
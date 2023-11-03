using Android.Content;
using Connect.Mobile.Interfaces;

namespace Connect.Mobile.Droid.Services
{
    public class ForegroundServiceAndroid : IForegroundService
    {
        private Intent Intent { get; set; }

        public void StartForegroundServiceCompat()
        {
            this.Intent = new Intent(MainActivity.Context, typeof(NotificationService));
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                MainActivity.Context.StartForegroundService(this.Intent);
            }
            else
            {
                MainActivity.Context.StartService(this.Intent);
            }
        }
    }
}
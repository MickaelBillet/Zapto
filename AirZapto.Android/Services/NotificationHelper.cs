using System;
using System.Diagnostics;
using AirZapto.Droid.Services;
using AirZapto.Mobile.Interfaces;
using Android.App;
using Android.Content;
using Android.Media;
using AndroidX.Core.App;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationHelper))]
namespace AirZapto.Droid.Services
{
	class NotificationHelper : INotification
    {
        public static string NOTIFICATION_CHANNEL_ID = "10023";

        #region Properties
        private Context Context { get; set; }

        private NotificationCompat.Builder Builder;

		#endregion

		#region Constructor

		public NotificationHelper()
        {
            Context = global::Android.App.Application.Context;
        }

		#endregion

		#region Methods

		public void CreateNotification(string title, string message)
        {
            try
            {
                Intent intent = new Intent(Context, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                intent.PutExtra(title, message);

                PendingIntent pendingIntent = PendingIntent.GetActivity(Context, 0, intent, PendingIntentFlags.OneShot);

                global::Android.Net.Uri sound = global::Android.Net.Uri.Parse(ContentResolver.SchemeAndroidResource + "://" + Context.PackageName + "/" + Resource.Raw.notification);

                // Creating an Audio Attribute
                var alarmAttributes = new AudioAttributes.Builder().SetContentType(AudioContentType.Sonification)
                                                                    .SetUsage(AudioUsageKind.Notification).Build();

                Builder = new NotificationCompat.Builder(Context);

                Builder.SetContentTitle(title)
                        .SetSound(sound)
                        .SetAutoCancel(true)
                        .SetContentTitle(title)
                        .SetContentText(message)
                        .SetChannelId(NOTIFICATION_CHANNEL_ID)
                        .SetPriority((int)NotificationPriority.Max)
                        .SetVibrate(new long[0])
                        .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
                        .SetVisibility((int)NotificationVisibility.Public)
                        .SetSmallIcon(Resource.Drawable.SplashScreen)
                        .SetContentIntent(pendingIntent);

                NotificationManager notificationManager = Context.GetSystemService(Context.NotificationService) as NotificationManager;

                if (global::Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.O)
                {
                    NotificationImportance importance = global::Android.App.NotificationImportance.High;

                    NotificationChannel notificationChannel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, title, importance);
                    notificationChannel.EnableLights(true);
                    notificationChannel.EnableVibration(true);
                    notificationChannel.SetSound(sound, alarmAttributes);
                    notificationChannel.SetShowBadge(true);
                    notificationChannel.Importance = NotificationImportance.Max;
                    notificationChannel.SetVibrationPattern(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 });

                    if (notificationManager != null)
                    {
                        Builder.SetChannelId(NOTIFICATION_CHANNEL_ID);
                        notificationManager.CreateNotificationChannel(notificationChannel);
                    }
                }

                notificationManager.Notify(0, Builder.Build());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

		#endregion
	}
}
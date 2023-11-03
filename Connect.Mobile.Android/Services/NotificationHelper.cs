using Android.App;
using Android.Content;
using Android.Media;
using Connect.Mobile.Droid.Services;
using Connect.Mobile.Interfaces;
using System;
using System.Diagnostics;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationHelper))]
namespace Connect.Mobile.Droid.Services
{
    class NotificationHelper : INotification
    {
        public static string NOTIFICATION_CHANNEL_ID = "10023";

        #region Properties
        private Context Context { get; set; }

        private Notification.Builder Builder;

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

                Builder = new Notification.Builder(Context, NOTIFICATION_CHANNEL_ID);

                Builder.SetContentTitle(title)
                        .SetAutoCancel(true)
                        .SetContentTitle(title)
                        .SetContentText(message)
                        .SetSound(sound)
                        .SetChannelId(NOTIFICATION_CHANNEL_ID)
                        .SetSmallIcon(Resource.Drawable.C)
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
using Android.App;
using Android.Content;
using Android.OS;
using Connect.Application.Infrastructure;
using Notification = Android.App.Notification;

namespace Connect.Mobile.Droid.Services
{
    [Service]
    class NotificationService : Service
    {
        internal static readonly string CHANNEL_ID = "Android_Service_Channel";
        internal static readonly int NOTIFICATION_ID = 936;

        private ISignalRService SignalRService { get; }

        public NotificationService()
        {
            this.SignalRService = Host.Current.GetService<ISignalRService>();
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(CHANNEL_ID, "Service Channel", NotificationImportance.High)
                {
                    Description = "Foreground Service Channel"
                };

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
                var pendingIntent = PendingIntent.GetActivity(this, NOTIFICATION_ID, new Intent(this, typeof(MainActivity)), PendingIntentFlags.Immutable);
                var notification = new Notification.Builder(this, CHANNEL_ID)
                .SetContentTitle("Connect Mobile")
                .SetContentText("Service Started")
                .SetContentIntent(pendingIntent)
                .SetSmallIcon(Resource.Drawable.C)
                .SetOngoing(true)
                .Build();
                StartForeground(NOTIFICATION_ID, notification);
            }

            //this.SignalRService.StartAsync(
            //            App.LocationId,
            //            null,
            //            null,
            //            null,
            //            (notificationStatus) =>
            //            {
            //                new NotificationHelper().CreateNotification(notificationStatus.Title, notificationStatus.Body);
            //            });

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
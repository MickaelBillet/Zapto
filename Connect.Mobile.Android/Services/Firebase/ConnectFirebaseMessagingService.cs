
using Android.App;
using Android.Content;
using Android.Util;
using Connect.Mobile.ViewModel;
using Connect.Model.Services;
using Firebase.Messaging;

namespace Connect.Mobile.Droid.Services
{
	[Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class ConnectFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "FirebaseIdService";

        #region Services 

        #endregion

        public ConnectFirebaseMessagingService()
        {
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            new NotificationHelper().CreateNotification(message.GetNotification().Title, message.GetNotification().Body);
        }

        public override async void OnNewToken(string token)
        {
            Log.Debug(TAG, "FCM token: " + token);
        }
    }
}
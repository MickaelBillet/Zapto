using Android.App;
using Android.Content;
using Android.OS;
using Connect.Application;
using Connect.Mobile.Interfaces;
using Connect.Model;
using Microsoft.Extensions.DependencyInjection;
using Plugin.PushNotification;
using System;
using Debug = System.Diagnostics.Debug;

namespace Connect.Mobile.Droid.Services
{
    public class FirebaseMobileService : IFirebaseMobileService
    {
        private string Topic { get; } = "General";
        private IApplicationClientAppServices ApplicationClientAppServices { get; }
        private string Token { get; set; }

        public FirebaseMobileService(IServiceProvider serviceProvider)
        {
            this.ApplicationClientAppServices = serviceProvider.GetRequiredService<IApplicationClientAppServices>();
        }

        public void Initialize(object application)
        {
            //Set the default notification channel for your app when running Android Oreo
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                PushNotificationManager.DefaultNotificationChannelId = "FirebasePushNotificationChannel";
                PushNotificationManager.DefaultNotificationChannelName = this.Topic;
                PushNotificationManager.DefaultNotificationChannelImportance = NotificationImportance.Max;
            }

#if DEBUG
            PushNotificationManager.Initialize(application as Android.App.Application, true);
#else
              PushNotificationManager.Initialize(application as Android.App.Application,false);
#endif
            PushNotificationManager.IconResource = Resource.Drawable.C;
        }

        public void LinkToActivity(object activity, object intent)
        {
            PushNotificationManager.ProcessIntent(activity as Activity, intent as Intent);
        }

        public void ReceiveNotification()
        {
            CrossPushNotification.Current.RegisterForPushNotifications();
            //Handle notification when app is closed here
            CrossPushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                try
                {
                    if ((p.Data != null) && (p.Data.ContainsKey("body")))
                    {
                        new NotificationHelper().CreateNotification(p.Data["title"].ToString(), p.Data["body"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            };
        }

        public void Subscribe(string locationId) 
        {
            CrossPushNotification.Current.OnTokenRefresh += async (s, p) =>
            {
                Debug.WriteLine($"TOKEN REC : {p.Token}");
                Debug.WriteLine($"LOCATIONID : {locationId}");

                this.Token = p.Token;

                try
                { 
                    if (await this.ApplicationClientAppServices.GetClientAppFromToken(p.Token) == null)
                    {
                        bool? res = await this.ApplicationClientAppServices.Save(new ClientApp()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Date = DateTime.Now,
                            LocationId = locationId,    
                            Description = "FirebaseClient",
                            Token = p.Token
                        });
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            };
        }

        public async void UnSubscribe() 
        {
            try
            {
                CrossPushNotification.Current.UnregisterForPushNotifications();
                bool? res = await this.ApplicationClientAppServices.Delete(this.Token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }
    }
}

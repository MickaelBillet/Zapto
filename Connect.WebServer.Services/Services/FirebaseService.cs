using Connect.Data;
using Connect.Model;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Message = FirebaseAdmin.Messaging.Message;
using Notification = FirebaseAdmin.Messaging.Notification;

namespace Connect.WebServer.Services
{
    public class FirebaseService : Application.Infrastructure.IFirebaseService
    {
        #region Properties
        ISupervisorClientApps Supervisor { get; }
        private static FirebaseApp? FirebaseApp { get; set; }
        #endregion

        #region Constructor
        public FirebaseService(IServiceScope scope, string fileConfigName)
        {
            if (FirebaseService.FirebaseApp == null)
            {
                //The Admin SDK also provides a credential which allows you to authenticate with a Google OAuth2 refresh token
                FirebaseService.FirebaseApp = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(fileConfigName),
                });
            }

            this.Supervisor = scope.ServiceProvider.GetRequiredService<ISupervisorClientApps>();

            Log.Information(FirebaseService.FirebaseApp.Name); // "[DEFAULT]"
        }
        #endregion

        #region Methods
        private async Task<string> SendNotificationAsync(string title, string body, string? token)
        {
            string id = string.Empty;

            try
            {
                //See documentation on defining a message payload.
                Message message = new Message()
                {
                    Notification = new Notification()
                    {
                        Title = title,
                        Body = body,
                    },
                    Token = token,
                };

                // Response is a message ID string.
                Log.Information("Successfully sent message : " + id);

                // Send a message to the device corresponding to the provided
                // registration token.
                id = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            }
            catch (Exception ex)
            {
                Log.Information("Cannot find the device : " + ex.Message);
            }

            return id;
        }

        public async Task SendAlertAsync(string locationId, string title, string body)
        {
            string response = string.Empty;

            try
            {
                IEnumerable<ClientApp>?clientApps = await this.Supervisor.GetClientApps();
                if (clientApps != null)
                {
                    foreach (ClientApp clientApp in clientApps)
                    {
                        if (clientApp.LocationId == locationId)
                        {
                            string id = await this.SendNotificationAsync(title, body, clientApp.Token);
                            if (string.IsNullOrEmpty(id))
                            {
                                Log.Information($"Impossible de notifier : {locationId}({title}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
        #endregion
    }
}

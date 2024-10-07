using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect.Application.Services
{
    internal sealed class ApplicationNotificationServices : IApplicationNotificationServices
    {
        #region Services

        private INotificationService? NotificationService { get; }

        #endregion

        #region Constructor

        public ApplicationNotificationServices(IServiceProvider serviceProvider)
        {
            this.NotificationService = serviceProvider.GetService<INotificationService>();
        }

        #endregion

        #region Methods

        public async Task<ICollection<Notification>?> GetNotifications(INotificationProvider provider)
        {
            if (this.NotificationService != null)
            {
                if (provider is ConnectedObject)
                {
                    return await this.NotificationService.GetFromConnectedObjectAsync(provider.Id);
                }
                else if (provider is Room)
                {
                    return await this.NotificationService.GetFromRoomAsync(provider.Id);
                }
            }

            return null;
        }

        public async Task<bool?> DeleteNotification(INotificationProvider provider, Notification notification)
        {
            bool? result = false;

            if ((this.NotificationService != null) && (await this.NotificationService.DeleteNotificationAsync(notification) == true))
            {
                result = provider.NotificationsList.Remove(notification);
            }

            return result;
        }

        public async Task<bool?> DeleteNotifications(INotificationProvider provider)
        {
            bool? result = null;

            if (this.NotificationService != null)
            {
                if (provider is ConnectedObject)
                {
                    result = await this.NotificationService.DeleteFromConnectedObjectAsync(provider.Id);
                }
                else if (provider is Room)
                {
                    result = await this.NotificationService.DeleteFromRoomAsync(provider.Id);
                }
            }
            
            if (result == true)
            {
                provider.NotificationsList.Clear();
            }

            return result;
        }

        public async Task<bool?> AddUpdateNotification(INotificationProvider provider, Notification notification)
        {
            bool? res = false;

            Notification? found = provider.NotificationsList.FirstOrDefault<Notification>((arg) => arg.Id == notification.Id);
            if (found != null)
            {
                res = (this.NotificationService != null) ? await this.NotificationService.UpdateNotificationAsync(notification) : null;

                if (res == true)
                {
                    res = provider.NotificationsList.Remove(found);
                }
            }
            else
            {
                if (provider is Room)
                {
                    notification.RoomId = provider.Id;

                }
                else if (provider is ConnectedObject)
                {
                    notification.ConnectedObjectId = provider.Id;
                }

                notification.Id = Guid.NewGuid().ToString();

                res = (this.NotificationService != null) ? await this.NotificationService.AddNotificationAsync(notification) : null;
            }

            if (res == true)
            {
                provider.NotificationsList.Add(notification);
            }

            return res;
        }

        #endregion
    }
}

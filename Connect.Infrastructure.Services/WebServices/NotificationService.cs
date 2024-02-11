using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Infrastructure.WebServices
{
    public class NotificationService : ConnectWebService, INotificationService
    {
        #region Constructor

        public NotificationService(IServiceProvider serviceProvider, string httpClientName) : base(serviceProvider, httpClientName)
        {

        }

        #endregion

        #region Method    

        public async Task<ObservableCollection<Notification>?> GetFromRoomAsync(string roomId, CancellationToken token = default)
        {
            IEnumerable<Notification>? notifications = await WebService.GetCollectionAsync<Notification>(string.Format(ConnectConstants.RestUrlRoomNotifications, roomId), SerializerOptions, token);
            return notifications != null ? new ObservableCollection<Notification>(notifications) : null;
        }

        public async Task<ObservableCollection<Notification>?> GetFromConnectedObjectAsync(string connectedObjectid, CancellationToken token = default)
        {
            IEnumerable<Notification>? notifications = await WebService.GetCollectionAsync<Notification>(string.Format(ConnectConstants.RestUrlConnectedObjectNotifications, connectedObjectid), SerializerOptions, token);
            return notifications != null ? new ObservableCollection<Notification>(notifications) : null;
        }

        public async Task<bool?> DeleteFromConnectedObjectAsync(string connectedObjectid, CancellationToken token = default)
        {
            return await WebService.DeleteAsync<Notification>(ConnectConstants.RestUrlConnectedObjectNotifications, connectedObjectid, token);;
        }

        public async Task<bool?> DeleteFromRoomAsync(string roomId, CancellationToken token = default)
        {
            return await WebService.DeleteAsync<Notification>(ConnectConstants.RestUrlRoomNotifications, roomId, token); ;
        }

        public async Task<bool?> DeleteNotificationAsync(Notification notification, CancellationToken token = default)
        {
            return await WebService.DeleteAsync<Notification>(ConnectConstants.RestUrlNotificationsId, notification.Id, token); ;
        }

        public async Task<bool?> AddNotificationAsync(Notification notification, CancellationToken token = default)
        {
            return await WebService.PostAsync<Notification>(ConnectConstants.RestUrlNotifications, notification, token); ;
        }

        public async Task<bool?> UpdateNotificationAsync(Notification notification, CancellationToken token = default)
        {
            return await WebService.PutAsync<Notification>(ConnectConstants.RestUrlNotificationsId, notification, notification.Id, token); ;
        }

        #endregion
    }
}

using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.Configuration;
using Serilog;
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

        public NotificationService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
        {

        }

        #endregion

        #region Method    

        public async Task<ObservableCollection<Notification>?> GetFromRoomAsync(string roomId, CancellationToken token = default)
        {
            IEnumerable<Notification>? notifications = null;

            try
            {
                notifications = await WebService.GetCollectionAsync<Notification>(string.Format(ConnectConstants.RestUrlRoomNotifications, roomId), SerializerOptions, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return notifications != null ? new ObservableCollection<Notification>(notifications) : null;
        }

        public async Task<ObservableCollection<Notification>?> GetFromConnectedObjectAsync(string connectedObjectid, CancellationToken token = default)
        {
            IEnumerable<Notification>? notifications = null;

            try
            {
                notifications = await WebService.GetCollectionAsync<Notification>(string.Format(ConnectConstants.RestUrlConnectedObjectNotifications, connectedObjectid), SerializerOptions, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return notifications != null ? new ObservableCollection<Notification>(notifications) : null;
        }

        public async Task<bool?> DeleteFromConnectedObjectAsync(string connectedObjectid, CancellationToken token = default)
        {
            bool? res = false;

            try
            {
                res = await WebService.DeleteAsync<Notification>(ConnectConstants.RestUrlConnectedObjectNotifications, connectedObjectid, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return res;
        }

        public async Task<bool?> DeleteFromRoomAsync(string roomId, CancellationToken token = default)
        {
            bool? res = false;

            try
            {
                res = await WebService.DeleteAsync<Notification>(ConnectConstants.RestUrlRoomNotifications, roomId, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return res;
        }

        public async Task<bool?> DeleteNotificationAsync(Notification notification, CancellationToken token = default)
        {
            bool? res = false;

            try
            {
                res = await WebService.DeleteAsync<Notification>(ConnectConstants.RestUrlNotificationsId, notification.Id, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return res;
        }

        public async Task<bool?> AddNotificationAsync(Notification notification, CancellationToken token = default)
        {
            bool? res = false;

            try
            {
                res = await WebService.PostAsync<Notification>(ConnectConstants.RestUrlNotifications, notification, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return res;
        }

        public async Task<bool?> UpdateNotificationAsync(Notification notification, CancellationToken token = default)
        {
            bool? res = false;

            try
            {
                res = await WebService.PutAsync<Notification>(ConnectConstants.RestUrlNotificationsId, notification, notification.Id, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return res;
        }

        #endregion
    }
}

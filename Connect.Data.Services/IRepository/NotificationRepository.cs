using Connect.Data.Interfaces;
using Connect.Data.Services;
using Connect.Model;
using Microsoft.Extensions.Configuration;
using Serilog;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Connect.Data.Repository
{
    internal sealed class NotificationRepository : IRepository<Notification>
    {
        #region Property

        private SQLiteAsyncConnection Connection => DbConnectionService.Service().GetDbConnection(this.Configuration);

        private IConfiguration Configuration { get; }

        #endregion

        #region Constructor

        public NotificationRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        #endregion

        #region Method

        /// <summary>
        /// Inserts the async.
        /// </summary>
        public async Task<int> InsertAsync(Notification notification)
        {
            int result = 0;

            try
            {
                if (notification != null)
                {
                    return await this.Connection.InsertAsync(notification);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Inserts the async.
        /// </summary>
        public async Task<int> InsertAsync(IEnumerable<Notification> items)
        {
            int result = 0;

            try
            {
                if (items != null)
                {
                    result = await this.Connection.InsertAllAsync(items, true);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Gets the async.
        /// </summary>
        public async Task<IEnumerable<Notification>> GetAsync()
        {
            try
            {
                return await this.Connection.Table<Notification>().ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<Notification>();
        }

        /// <summary>
        /// Get item
        /// </summary>
        public async Task<Notification> GetAsync(string id)
        {
            try
            {
                return await this.Connection.Table<Notification>().Where((Notification arg) => arg.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Gets the async.
        /// </summary>
        public async Task<IEnumerable<Notification>> GetAsync<TValue>(Expression<Func<Notification, bool>> predicate)
        {
            try
            {
                return await this.Connection.Table<Notification>().Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<Notification>();
        }

        /// <summary>
        /// Gets the async.
        /// </summary>
        public async Task<Notification> GetAsync(Expression<Func<Notification, bool>> predicate)
        {
            try
            {
                return await this.Connection.Table<Notification>().Where(predicate).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Update Notification
        /// </summary>
        public async Task<int> UpdateAsync(Notification item)
        {
            int result = 0;

            try
            { 
                if (item != null)
                {
                    result = await this.Connection.UpdateAsync(item);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Deletes the async.
        /// </summary>
        public async Task<int> DeleteAsync(Notification item)
        {
            int res = 0;

            try
            { 
                if (item != null)
                {
                    res = await this.Connection.DeleteAsync<Notification>(item.Id);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return res;
        }

        /// <summary>
        /// Deletes the async.
        /// </summary>
        public async Task<int> DeleteAsync(IEnumerable<Notification> items)
        {
            int res = 0;

            try
            {
                if (items != null)
                {
                    foreach (Notification notification in items)
                    {
                        res = await this.Connection.DeleteAsync<Notification>(notification.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return res;
        }

        public async void Dispose()
        {
            await this.Connection.CloseAsync();
        }

        #endregion
    }
}
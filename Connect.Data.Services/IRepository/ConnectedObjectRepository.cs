using Connect.Data.Interfaces;
using Connect.Data.Services;
using Connect.Model;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Connect.Data.Repository
{
    internal sealed class ConnectedObjectRepository : IRepository<ConnectedObject>
    {
        #region Property

        private IDbConnectionService Connection { get; }

        #endregion

        #region Constructor

        public ConnectedObjectRepository(IServiceProvider serviceProvider)
        {
           this.Connection = serviceProvider.GetRequiredService<IDbConnectionService>();
        }

        #endregion

        #region Method

        /// <summary>
        /// Inserts the async.
        /// </summary>
        /// <returns>The async.</returns>
        public async Task<int> InsertAsync(ConnectedObject item)
        {
            int result = 0;

            try
            {
                if (item != null)
                {
                    result = await this.Connection.GetDbConnection().InsertAsync(item);
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
        /// <returns>The async.</returns>
        /// <param name="items">Items.</param>
        public async Task<int> InsertAsync(IEnumerable<ConnectedObject> items)
        {
            int result = 0;

            try
            {
                if (items != null)
                {
                    result = await this.Connection.GetDbConnection().InsertAllAsync(items, true);
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
        /// <returns>The async.</returns>
        public async Task<IEnumerable<ConnectedObject>> GetAsync()
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<ConnectedObject>().ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<ConnectedObject>();
        }

        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="id">Identifier.</param>
        public async Task<ConnectedObject> GetAsync(string id)
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<ConnectedObject>().FirstOrDefaultAsync((ConnectedObject arg) => arg.Id == id);
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
        /// <returns>The async.</returns>
        /// <param name="predicate">Predicate.</param>
        /// <typeparam name="TValue">The 1st type parameter.</typeparam>
        public async Task<IEnumerable<ConnectedObject>> GetAsync<TValue>(Expression<Func<ConnectedObject, bool>> predicate)
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<ConnectedObject>().Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<ConnectedObject>();
        }

        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="predicate">Predicate.</param>
        public async Task<ConnectedObject> GetAsync(Expression<Func<ConnectedObject, bool>> predicate)
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<ConnectedObject>().FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Updates the async.
        /// </summary>
        /// <returns>The async.</returns>
		public async Task<int> UpdateAsync(ConnectedObject item)
        {
            int res = 0;

            try
            { 
                if (item != null)
                {
                    res = await this.Connection.GetDbConnection().UpdateAsync(item);
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
        /// <returns>The async.</returns>
		public async Task<int> DeleteAsync(ConnectedObject item)
		{
            int res = 0;

            try
            { 
                if (item != null)
                {
                    res = await this.Connection.GetDbConnection().DeleteAsync<ConnectedObject>(item.Id);
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
        /// <returns>The async.</returns>
        public async Task<int> DeleteAsync(IEnumerable<ConnectedObject> items)
        {
            int res = 0;

            try
            {
                if (items != null)
                {
                    foreach (ConnectedObject item in items)
                    {
                        res = await this.Connection.GetDbConnection().DeleteAsync<Condition>(item.Id);
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
            await this.Connection.GetDbConnection().CloseAsync();
        }

        #endregion
	}
}
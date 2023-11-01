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
    internal sealed class ClientAppRepository : IRepository<ClientApp>
    {
        #region Property

        private IDbConnectionService Connection { get; }

        #endregion

        #region Constructor

        public ClientAppRepository(IServiceProvider serviceProvider)
        {
            this.Connection = serviceProvider.GetRequiredService<IDbConnectionService>();
        }

        #endregion

        #region Method

        /// <summary>
        /// Inserts the async.
        /// </summary>
        public async Task<int> InsertAsync(ClientApp clientApp)
        {
            int result = 0;

            try
            {
                if (clientApp != null)
                {
                    return await this.Connection.GetDbConnection().InsertAsync(clientApp);
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
        public async Task<int> InsertAsync(IEnumerable<ClientApp> items)
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
        public async Task<IEnumerable<ClientApp>> GetAsync()
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<ClientApp>().ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<ClientApp>();
        }

        /// <summary>
        /// Get Sensor
        /// </summary>
        public async Task<ClientApp> GetAsync(string id)
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<ClientApp>().Where((ClientApp arg) => arg.Id == id).FirstOrDefaultAsync();
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
        public async Task<IEnumerable<ClientApp>> GetAsync<TValue>(Expression<Func<ClientApp, bool>> predicate)
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<ClientApp>().Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<ClientApp>();
        }

        /// <summary>
        /// Gets the async.
        /// </summary>
        public async Task<ClientApp> GetAsync(Expression<Func<ClientApp, bool>> predicate)
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<ClientApp>().Where(predicate).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Update Program
        /// </summary>
        public async Task<int> UpdateAsync(ClientApp clientApp)
        {
            int result = 0;

            try
            {
                if (clientApp != null)
                {
                    result = await this.Connection.GetDbConnection().UpdateAsync(clientApp);
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
        public async Task<int> DeleteAsync(ClientApp clientApp)
        {
            int res = 0;

            try
            {
                if (clientApp != null)
                {
                    res = await this.Connection.GetDbConnection().DeleteAsync<ClientApp>(clientApp.Id);
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
        public async Task<int> DeleteAsync(IEnumerable<ClientApp> items)
        {
            int res = 0;

            try
            {
                if (items != null)
                {
                    foreach (ClientApp clientApp in items)
                    {
                        res = await this.Connection.GetDbConnection().DeleteAsync<ClientApp>(clientApp.Id);
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
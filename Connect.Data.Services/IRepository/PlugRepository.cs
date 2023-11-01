using Connect.Data.Interfaces;
using Connect.Data.Services;
using Connect.Model;
using Microsoft.Extensions.Configuration;
using Serilog;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Connect.Data.Repository
{
    internal sealed class PlugRepository : IRepository<Plug>
    {
        #region Property

        private SQLiteAsyncConnection Connection => DbConnectionService.Service().GetDbConnection(this.Configuration);

        private IConfiguration Configuration { get; }

        #endregion

        #region Constructor

        public PlugRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        #endregion

        #region Method

        /// <summary>
        /// Insert Plug
        /// </summary>
        /// <param name="plug"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(Plug plug)
        {
            int result = 0;

            try
            {
                if (plug != null)
                {
                    result = await this.Connection.InsertAsync(plug);
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
        /// <param name="plugs">Plugs.</param>
        public async Task<int> InsertAsync(IEnumerable<Plug> plugs)
        {
            int result = 0;

            try
            {
                if (plugs != null)
                {
                    result = await this.Connection.InsertAllAsync(plugs, true);
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
        public async Task<IEnumerable<Plug>> GetAsync()
        {
            try
            {
                return await this.Connection.Table<Plug>().ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<Plug>();
        }

        /// <summary>
        /// Get Plug
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Plug> GetAsync(string id)
        {
            try
            {
                return await this.Connection.Table<Plug>().FirstOrDefaultAsync((Plug arg) => arg.Id == id);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Get Plug list
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Plug>> GetAsync<TValue>(Expression<Func<Plug, bool>> predicate)
        {
            try
            {
               return await this.Connection.Table<Plug>().Where(predicate).OrderBy((Plug plug) => plug.Type).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<Plug>();
        }

        /// <summary>
        /// Get Plug
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<Plug> GetAsync(Expression<Func<Plug, bool>> predicate)
        {
            try
            {
                return await this.Connection.Table<Plug>().FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Update Plug
        /// </summary>
        /// <param name="plug"></param>
        /// <returns></returns>
		public async Task<int> UpdateAsync(Plug plug)
        {
            int res = 0;

            try
            { 
                if (plug != null)
                {
                    res = await this.Connection.UpdateAsync(plug);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return res;
        }

        /// <summary>
        /// Delete Plug
        /// </summary>
        /// <param name="plug"></param>
        /// <returns></returns>
		public async Task<int> DeleteAsync(Plug plug)
		{
            int res = 0;

            try
            { 
                if (plug != null)
                {
                    res = await this.Connection.DeleteAsync<Plug>(plug.Id);
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
        /// <param name="plugs">Plugs.</param>
        public async Task<int> DeleteAsync(IEnumerable<Plug> plugs)
        {
            int res = 0;

            try
            {
                if (plugs != null)
                {
                    foreach (Plug plug in plugs)
                    {
                        res = await this.Connection.DeleteAsync<Plug>(plug.Id);
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
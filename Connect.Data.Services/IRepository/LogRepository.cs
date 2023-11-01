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
	internal sealed class LogRepository : IRepository<Logs>
    {
        #region Property

        private SQLiteAsyncConnection Connection => DbConnectionService.Service().GetDbConnection(this.Configuration);

        private IConfiguration Configuration { get; }

        #endregion

        #region Constructor

        public LogRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        #endregion

        #region Method

        public async Task<int> InsertAsync(Logs log)
        {
            int result = 0;

            try
            {
                if (log != null)
                {
                    int index = (await this.Connection.Table<Logs>().ToListAsync()).Max(log => log.id);
                    log.id = index + 1;
                    result = await this.Connection.InsertAsync(log);
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
        public async Task<int> InsertAsync(IEnumerable<Logs> logs)
        {
            int result = 0;

            try
            {
                if (logs != null)
                {
                    result = await this.Connection.InsertAllAsync(logs, true);
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
        public async Task<IEnumerable<Logs>> GetAsync()
        {
            try
            {
                return await this.Connection.Table<Logs>().ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<Logs>();
        }

        /// <summary>
        /// Get Log
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Logs> GetAsync(string id)
        {
            try
            {
                return await this.Connection.Table<Logs>().FirstOrDefaultAsync((Logs arg) => arg.id == int.Parse(id));
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Get Logs list
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Logs>> GetAsync<TValue>(Expression<Func<Logs, bool>> predicate)
        {
            try
            {
               return await this.Connection.Table<Logs>().Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<Logs>();
        }

        /// <summary>
        /// Get Logs
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<Logs> GetAsync(Expression<Func<Logs, bool>> predicate)
        {
            try
            {
                return await this.Connection.Table<Logs>().FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Update Logs
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
		public async Task<int> UpdateAsync(Logs log)
        {
            int res = 0;

            try
            {
                if (log != null)
                {
                    res = await this.Connection.UpdateAsync(log);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return res;
        }

        /// <summary>
        /// Delete Logs
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
		public async Task<int> DeleteAsync(Logs log)
        {
            int res = 0;

            try
            {
                if (log != null)
                {
                    res = await this.Connection.DeleteAsync<Logs>(log.id);
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
        /// <param name="logs">Logs.</param>
        public async Task<int> DeleteAsync(IEnumerable<Logs> logs)
        {
            int res = 0;

            try
            {
                if (logs != null)
                {
                    foreach (Logs log in logs)
                    {
                        res = await this.Connection.DeleteAsync<Logs>(log.id);
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
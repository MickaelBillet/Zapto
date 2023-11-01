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
    internal sealed class SensorRepository : IRepository<Sensor>
    {
        #region Property

        private SQLiteAsyncConnection Connection => DbConnectionService.Service().GetDbConnection(this.Configuration);

        private IConfiguration Configuration { get; }

        #endregion

        #region Constructor

        public SensorRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        #endregion

        #region Method

        /// <summary>
        /// Inserts the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="program">Program.</param>
        public async Task<int> InsertAsync(Sensor sensor)
        {
            int result = 0;

            try
            {
                if (sensor != null)
                {
                    return await this.Connection.InsertAsync(sensor);
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
        public async Task<int> InsertAsync(IEnumerable<Sensor> items)
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
        /// <returns>The async.</returns>
        public async Task<IEnumerable<Sensor>> GetAsync()
        {
            try
            {
                return await this.Connection.Table<Sensor>().ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<Sensor>();
        }

        /// <summary>
        /// Get Sensor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Sensor> GetAsync(String id)
        {
            try
            {
                return await this.Connection.Table<Sensor>().Where((Sensor arg) => arg.Id == id).FirstOrDefaultAsync();
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
        public async Task<IEnumerable<Sensor>> GetAsync<TValue>(Expression<Func<Sensor, bool>> predicate)
        {
            try
            {
                return await this.Connection.Table<Sensor>().Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<Sensor>();
        }

        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="predicate">Predicate.</param>
        public async Task<Sensor> GetAsync(Expression<Func<Sensor, bool>> predicate)
        {
            try
            {
                return await this.Connection.Table<Sensor>().Where(predicate).FirstOrDefaultAsync();
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
        /// <param name="program"></param>
        /// <returns></returns>
		public async Task<int> UpdateAsync(Sensor sensor)
        {
            int result = 0;

            try
            { 
                if (sensor != null)
                {
                    result = await this.Connection.UpdateAsync(sensor);
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
        /// <returns>The async.</returns>
        /// <param name="program">Program.</param>
		public async Task<int> DeleteAsync(Sensor sensor)
		{
            int res = 0;

            try
            { 
                if (sensor != null)
                {
                    res = await this.Connection.DeleteAsync<Sensor>(sensor.Id);
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
        /// <param name="programs">Programs.</param>
        public async Task<int> DeleteAsync(IEnumerable<Sensor> sensors)
        {
            int res = 0;

            try
            {
                if (sensors != null)
                {
                    foreach (Sensor sensor in sensors)
                    {
                        res = await this.Connection.DeleteAsync<Sensor>(sensor.Id);
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
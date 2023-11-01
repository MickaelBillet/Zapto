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
    internal sealed class OperatingDataRepository : IRepository<OperatingData>
    {
        #region Property

        private SQLiteAsyncConnection Connection => DbConnectionService.Service().GetDbConnection(this.Configuration);

        private IConfiguration Configuration { get; }

        #endregion

        #region Constructor

        public OperatingDataRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        #endregion

        #region Method

        /// <summary>
        /// Inserts the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="operatingData">Operating data.</param>
        public async Task<int> InsertAsync(OperatingData operatingData)
        {
            int result = 0;

            try
            {
                if (operatingData != null)
                {
                    result = await this.Connection.GetDbConnection().InsertAsync(operatingData);
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
        public async Task<int> InsertAsync(IEnumerable<OperatingData> items)
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
        public async Task<IEnumerable<OperatingData>> GetAsync()
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<OperatingData>().ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<OperatingData>();
        }

        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="id">Identifier.</param>
        public async Task<OperatingData> GetAsync(String id)
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<OperatingData>().FirstOrDefaultAsync((OperatingData arg) => arg.Id == id);
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
        public async Task<IEnumerable<OperatingData>> GetAsync<TValue>(Expression<Func<OperatingData, bool>> predicate)
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<OperatingData>().Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<OperatingData>();
        }

        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="predicate">Predicate.</param>
        public async Task<OperatingData> GetAsync(Expression<Func<OperatingData, bool>> predicate)
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<OperatingData>().FirstOrDefaultAsync(predicate);
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
        /// <param name="operatingData">Operating data.</param>
		public async Task<int> UpdateAsync(OperatingData operatingData)
        {
            int res = 0;

            try
            { 
                if (operatingData != null)
                {
                    res = await this.Connection.GetDbConnection().UpdateAsync(operatingData);
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
        /// <param name="operatingData">Operating data.</param>
		public async Task<int> DeleteAsync(OperatingData operatingData)
		{
            int res = 0;

            try
            { 
                if (operatingData != null)
                {
                    res = await this.Connection.GetDbConnection().DeleteAsync<OperatingData>(operatingData.Id);
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
        /// <param name="operatingDatas">Operating datas.</param>
        public async Task<int> DeleteAsync(IEnumerable<OperatingData> operatingDatas)
        {
            int res = 0;

            try
            {
                if (operatingDatas != null)
                {
                    foreach (OperatingData operatingData in operatingDatas)
                    {
                        res = res + await this.Connection.GetDbConnection().DeleteAsync<OperatingData>(operatingData.Id);
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
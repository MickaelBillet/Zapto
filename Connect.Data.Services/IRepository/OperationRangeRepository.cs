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
    internal sealed class OperationRangeRepository : IRepository<OperationRange>
    {
        #region Property

        private SQLiteAsyncConnection Connection => DbConnectionService.Service().GetDbConnection(this.Configuration);

        private IConfiguration Configuration { get; }

        #endregion

        #region Constructor

        public OperationRangeRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        #endregion

        #region Method

        /// <summary>
        /// Insert OperationRange
        /// </summary>
        /// <param name="operationRange"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(OperationRange operationRange)
        {
            int result = 0;

            try
            {
                if (operationRange != null)
                {
                    result = await this.Connection.InsertAsync(operationRange);
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
        /// <param name="operationRanges">Operation ranges.</param>
        public async Task<int> InsertAsync(IEnumerable<OperationRange> operationRanges)
        {
            int result = 0;

            try
            {
                if (operationRanges != null)
                {
                    result = await this.Connection.InsertAllAsync(operationRanges, true);
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
        public async Task<IEnumerable<OperationRange>> GetAsync()
        {
            try
            {
                return await this.Connection.Table<OperationRange>().ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<OperationRange>();
        }

        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="id">Identifier.</param>
        public async Task<OperationRange> GetAsync(String id)
        {
            try
            {
                return await this.Connection.Table<OperationRange>().FirstOrDefaultAsync((OperationRange arg) => arg.Id == id);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Get OperationRange list
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<OperationRange>> GetAsync<TValue>(Expression<Func<OperationRange, bool>> predicate)
        {
            try
            {
                return await this.Connection.Table<OperationRange>().Where(predicate).OrderBy((OperationRange operationRange) => operationRange.Day).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<OperationRange>();
        }

        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="predicate">Predicate.</param>
        public async Task<OperationRange> GetAsync(Expression<Func<OperationRange, bool>> predicate)
        {
            try
            {
                return await this.Connection.Table<OperationRange>().FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Update OperationRange
        /// </summary>
        /// <param name="operationRange"></param>
        /// <returns></returns>
		public async Task<int> UpdateAsync(OperationRange operationRange)
        {
            int res = 0;

            try
            { 
                if (operationRange != null)
                {
                    return await this.Connection.UpdateAsync(operationRange);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return res;
        }

        /// <summary>
        /// Delete OperationRange
        /// </summary>
        /// <param name="operationRange"></param>
        /// <returns></returns>
		public async Task<int> DeleteAsync(OperationRange operationRange)
		{
            int res = 0;

            try
            { 
                if (operationRange != null)
                {
                    return await this.Connection.DeleteAsync<OperationRange>(operationRange.Id);
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
        /// <param name="operationRanges">Operation ranges.</param>
        public async Task<int> DeleteAsync(IEnumerable<OperationRange> operationRanges)
        {
            int res = 0;

            try
            {
                if (operationRanges != null)
                {
                    foreach(OperationRange operationRange in operationRanges)
                    {
                        res = res + await this.Connection.DeleteAsync<OperationRange>(operationRange.Id);
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
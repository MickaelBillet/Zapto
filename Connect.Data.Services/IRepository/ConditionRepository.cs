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
    internal sealed class ConditionRepository : IRepository<Condition>
    {
        #region Property

        private IDbConnectionService Connection { get; }

        #endregion

        #region Constructor

        public ConditionRepository(IServiceProvider serviceProvider)
        {
            this.Connection = serviceProvider.GetRequiredService<IDbConnectionService>();
        }

        #endregion

        #region Method

        /// <summary>
        /// Inserts the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="condition">Condition.</param>
        public async Task<int> InsertAsync(Condition condition)
        {
            int result = 0;

            try
            {
                if (condition != null)
                {
                    result = await this.Connection.GetDbConnection().InsertAsync(condition);
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
        public async Task<int> InsertAsync(IEnumerable<Condition> items)
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
        public async Task<IEnumerable<Condition>> GetAsync()
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<Condition>().ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<Condition>();
        }

        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="id">Identifier.</param>
        public async Task<Condition> GetAsync(String id)
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<Condition>().FirstOrDefaultAsync((Condition arg) => arg.Id == id);
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
        public async Task<IEnumerable<Condition>> GetAsync<TValue>(Expression<Func<Condition, bool>> predicate)
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<Condition>().Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<Condition>();
        }

        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="predicate">Predicate.</param>
        public async Task<Condition> GetAsync(Expression<Func<Condition, bool>> predicate)
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<Condition>().FirstOrDefaultAsync(predicate);
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
        /// <param name="condition">Condition.</param>
		public async Task<int> UpdateAsync(Condition condition)
        {
            int res = 0;

            try
            { 
                if (condition != null)
                {
                    res = await this.Connection.GetDbConnection().UpdateAsync(condition);
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
        /// <param name="condition">Condition.</param>
		public async Task<int> DeleteAsync(Condition condition)
		{
            int res = 0;

            try
            { 
                if (condition != null)
                {
                    res = await this.Connection.GetDbConnection().DeleteAsync<Condition>(condition.Id);
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
        /// <param name="conditions">Conditions.</param>
        public async Task<int> DeleteAsync(IEnumerable<Condition> conditions)
        {
            int res = 0;

            try
            {
                if (conditions != null)
                {
                    foreach (Condition condition in conditions)
                    {
                        res = await this.Connection.GetDbConnection().DeleteAsync<Condition>(condition.Id);
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
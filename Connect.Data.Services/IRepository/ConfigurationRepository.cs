using Connect.Data.Interfaces;
using Connect.Data.Services;
using Connect.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    internal sealed class ConfigurationRepository : IRepository<Configuration>
    {
        #region Property

        private IDbConnectionService Connection { get; }

        #endregion

        #region Constructor

        public ConfigurationRepository(IServiceProvider serviceProvider)
        {
            this.Connection = serviceProvider.GetRequiredService<IDbConnectionService>();
        }

        #endregion

        #region Method

        /// <summary>
        /// Inserts the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="config">Config.</param>
        public async Task<int> InsertAsync(Configuration config)
        {
            int result = 0;

            try
            {
                if (config != null)
                {
                    result = await this.Connection.GetDbConnection().InsertAsync(config);
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
        public async Task<int> InsertAsync(IEnumerable<Configuration> items)
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
        public async Task<IEnumerable<Configuration>> GetAsync()
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<Configuration>().ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<Configuration>();
        }

        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="id">Identifier.</param>
        public async Task<Configuration> GetAsync(String id)
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<Configuration>().FirstOrDefaultAsync((Configuration config) => config.Id == id);
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
        public async Task<IEnumerable<Configuration>> GetAsync<TValue>(Expression<Func<Configuration, bool>> predicate)
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<Configuration>().Where(predicate).OrderBy((Configuration config) => config.ProtocolType).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<Configuration>();
        }

        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="predicate">Predicate.</param>
        public async Task<Configuration> GetAsync(Expression<Func<Configuration, bool>> predicate)
        {
            try
            {
                return await this.Connection.GetDbConnection().Table<Configuration>().FirstOrDefaultAsync(predicate);
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
        /// <param name="config">Config.</param>
		public async Task<int> UpdateAsync(Configuration config)
        {
            int res = 0;

            try
            { 
                if (config != null)
                {
                    res = await this.Connection.GetDbConnection().UpdateAsync(config);
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
        /// <param name="config">Config.</param>
		public async Task<int> DeleteAsync(Configuration config)
		{
            int res = 0;

            try
            { 
                if (config != null)
                {
                    res = await this.Connection.GetDbConnection().DeleteAsync<Configuration>(config.Id);
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
        /// <param name="configurations">Configurations.</param>
        public async Task<int> DeleteAsync(IEnumerable<Configuration> configurations)
        {
            int res = 0;

            try
            {
                if (configurations != null)
                {
                    foreach (Configuration configuration in configurations)
                    {
                        res = res + await this.Connection.GetDbConnection().DeleteAsync<Configuration>(configuration.Id);
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
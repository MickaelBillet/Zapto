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
    internal sealed class LocationRepository : IRepository<Location>
    {
        #region Property

        private IDbConnectionService Connection { get; }

        #endregion

        #region Constructor

        public LocationRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        #endregion

        #region Method

        /// <summary>
        /// Inserts the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="location">Location.</param>
        public async Task<int> InsertAsync(Location location)
        {
            int result = 0;

            try
            {
                if (location != null)
                {
                    result = await this.Connection.InsertAsync(location);
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
        public async Task<int> InsertAsync(IEnumerable<Location> items)
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
        public async Task<IEnumerable<Location>> GetAsync()
        {
            try
            {
                return await this.Connection.Table<Location>().ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<Location>();
        }

        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="id">Identifier.</param>
        public async Task<Location> GetAsync(string id)
        {
            try
            {
                return await this.Connection.Table<Location>().FirstOrDefaultAsync((Location arg) => arg.Id == id);
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
        public async Task<IEnumerable<Location>> GetAsync<TValue>(Expression<Func<Location, bool>> predicate)
        {
            try
            {
                return await this.Connection.Table<Location>().Where(predicate).OrderBy((Location location)=>location.City).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<Location>();
        }

        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="predicate">Predicate.</param>
        public async Task<Location> GetAsync(Expression<Func<Location, bool>> predicate)
        {
            try
            {
                return await this.Connection.Table<Location>().FirstOrDefaultAsync(predicate);
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
        /// <param name="location">Location.</param>
		public async Task<int> UpdateAsync(Location location)
        {
            int res = 0;

            try
            { 
                if (location != null)
                {
                    res = await this.Connection.UpdateAsync(location);
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
        /// <param name="location">Location.</param>
		public async Task<int> DeleteAsync(Location location)
		{
            int res = 0;

            try
            { 
                if (location != null)
                {
                    res = await this.Connection.DeleteAsync<Location>(location.Id);
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
        /// <param name="locations">Locations.</param>
        public async Task<int> DeleteAsync(IEnumerable<Location> locations)
        {
            int res = 0;

            try
            {
                if (locations != null)
                {
                    foreach (Location location in locations)
                    {
                        res = res + await this.Connection.DeleteAsync<Location>(location.Id);
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
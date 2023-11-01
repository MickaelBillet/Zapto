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
    internal sealed class ProgramRepository : IRepository<Program>
    {
        #region Property

        private SQLiteAsyncConnection Connection => DbConnectionService.Service().GetDbConnection(this.Configuration);

        private IConfiguration Configuration { get; }

        #endregion

        #region Constructor

        public ProgramRepository(IConfiguration configuration)
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
        public async Task<int> InsertAsync(Program program)
        {
            int result = 0;

            try
            {
                if (program != null)
                {
                    return await this.Connection.InsertAsync(program);
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
        public async Task<int> InsertAsync(IEnumerable<Program> items)
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
        public async Task<IEnumerable<Program>> GetAsync()
        {
            try
            {
                return await this.Connection.Table<Program>().ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<Program>();
        }

        /// <summary>
        /// Get Program
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Program> GetAsync(String id)
        {
            try
            {
                return await this.Connection.Table<Program>().Where((Program arg) => arg.Id == id).FirstOrDefaultAsync();
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
        public async Task<IEnumerable<Program>> GetAsync<TValue>(Expression<Func<Program, bool>> predicate)
        {
            try
            {
                return await this.Connection.Table<Program>().Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<Program>();
        }

        /// <summary>
        /// Gets the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="predicate">Predicate.</param>
        public async Task<Program> GetAsync(Expression<Func<Program, bool>> predicate)
        {
            try
            {
                return await this.Connection.Table<Program>().Where(predicate).FirstOrDefaultAsync();
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
		public async Task<int> UpdateAsync(Program program)
        {
            int result = 0;

            try
            { 
                if (program != null)
                {
                    result = await this.Connection.UpdateAsync(program);
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
		public async Task<int> DeleteAsync(Program program)
		{
            int res = 0;

            try
            { 
                if (program != null)
                {
                    res = await this.Connection.DeleteAsync<Program>(program.Id);
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
        public async Task<int> DeleteAsync(IEnumerable<Program> programs)
        {
            int res = 0;

            try
            {
                if (programs != null)
                {
                    foreach (Program program in programs)
                    {
                        res = await this.Connection.DeleteAsync<Program>(program.Id);
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
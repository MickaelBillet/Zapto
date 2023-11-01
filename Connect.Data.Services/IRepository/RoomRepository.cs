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
    internal sealed class RoomRepository : IRoomRepository
    {
        #region Property

        private SQLiteAsyncConnection Connection => DbConnectionService.Service().GetDbConnection(this.Configuration);

        private IConfiguration Configuration { get; }

        #endregion

        #region Constructor

        public RoomRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        #endregion

        #region Method

        /// <summary>
        /// Insert Room
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(Room room)
        {
            int result = 0;

            try
            {
                if (room != null)
                {
                    result = await this.Connection.InsertAsync(room);
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
        public async Task<int> InsertAsync(IEnumerable<Room> items)
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

        public async Task<IEnumerable<Room>> GetAsync()
        {
            try
            {
                return await this.Connection.Table<Room>().ToListAsync(); 
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<Room>();
        }

        /// <summary>
        /// Get Room
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Room> GetAsync(String id)
        {
            try
            {
                return await this.Connection.Table<Room>().FirstOrDefaultAsync((Room arg) => arg.Id == id);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return null;
        }

        public async Task<Room> GetFromPlugIdAsync(string plugId)
        {
            try
            {
                return (await this.Connection.QueryAsync<Room>("SELECT * FROM room INNER JOIN connectedObject ON connectedObject.RoomId = room.Id " 
                                                                                    + "INNER JOIN plug ON connectedobject.Id == plug.ConnectedObjectId "
                                                                                    + "WHERE plug.Id=?", plugId)).FirstOrDefault<Room>();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Get Room list
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Room>> GetAsync<TValue>(Expression<Func<Room, bool>> predicate)
        {
            try
            {
               return await this.Connection.Table<Room>().Where(predicate).OrderBy((Room room) => room.Name).ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.Message);
            }

            return Enumerable.Empty<Room>();
        }

        /// <summary>
        /// Get Room
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<Room> GetAsync(Expression<Func<Room, bool>> predicate)
        {
            try
            {
                return await this.Connection.Table<Room>().FirstOrDefaultAsync(predicate);
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
        /// <param name="room">Room.</param>
		public async Task<int> UpdateAsync(Room room)
        {
            int res = 0;

            try
            { 
                if (room != null)
                {
                    res = await this.Connection.UpdateAsync(room);
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
        /// <param name="room">Room.</param>
		public async Task<int> DeleteAsync(Room room)
		{
            int res = 0;

            try
            { 
                if (room != null)
                {
                    res = await this.Connection.DeleteAsync<Room>(room.Id);
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
        /// <param name="rooms">Rooms.</param>
        public async Task<int> DeleteAsync(IEnumerable<Room> rooms)
        {
            int res = 0;

            try
            {
                if (rooms != null)
                {
                    foreach (Room room in rooms)
                    {
                        res = await this.Connection.DeleteAsync<Room>(room.Id);
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
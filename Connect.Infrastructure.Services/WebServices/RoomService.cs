using Connect.Application.Infrastructure;
using Connect.Model;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Connect.Infrastructure.WebServices
{
    public class RoomService : ConnectWebService, IRoomService
    {
        #region Property

        private ICacheService? CacheService { get; }

        #endregion

        #region Constructor

        public RoomService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
        {
            CacheService = serviceProvider.GetService<ICacheService>();
        }

        #endregion

        #region Method

        public async Task<IEnumerable<Room>?> GetRooms(string? locationId, CancellationToken token = default)
        {
            IEnumerable<Room>? rooms = null;

            try
            {
                rooms = await WebService.GetCollectionAsync<Room>(string.Format(ConnectConstants.RestUrlLocationRooms, locationId), SerializerOptions, token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return rooms;
        }

        public IObservable<Room> GetRoom(string roomId, bool forceRefresh = true)
        {
            return Observable.Create<Room>(async (observer) =>
            {
                try
                {
                    Room? room = null;

                    try
                    {
                        room = CacheService != null ? await CacheService.GetObject<Room>("room" + roomId) : null;
                    }
                    finally
                    {
                        if (room != null)
                        {
                            observer.OnNext(room);
                        }

                        if (forceRefresh)
                        {
                            //Call the webservice
                            room = await WebService.GetAsync<Room>(ConnectConstants.RestUrlRoomsId, roomId, SerializerOptions);
                            if (room != null)
                            {
                                if (CacheService != null)
                                {
                                    //Insert the objets in to the database
                                    await CacheService.InsertObject<Room>("room" + room.Id, room);
                                }

                                observer.OnNext(room);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                    Log.Error(ex.Message);
                }

                observer.OnCompleted();
            });
        }

        #endregion
    }
}

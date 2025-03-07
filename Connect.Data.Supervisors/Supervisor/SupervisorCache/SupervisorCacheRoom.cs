﻿using Connect.Data.Mappers;
using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorCacheRoom : SupervisorCache, ISupervisorRoom
    {
        #region Services
        private ISupervisorRoom Supervisor { get; }
        #endregion

        #region Constructor
        public SupervisorCacheRoom(IServiceProvider serviceProvider) : base (serviceProvider)
        {
            this.Supervisor = serviceProvider.GetRequiredService<ISupervisorFactoryRoom>().CreateSupervisor(CacheType.None);    
        }
        #endregion

        #region Methods
        public async Task Initialize()
        {
            IEnumerable<Room> rooms = await this.Supervisor.GetRooms();
            foreach (var item in rooms)
            {
                await this.CacheRoomService.Set(item.Id, item);
            }
        }
        public async Task<IEnumerable<Room>> GetRooms()
        {
            List<Room> rooms = (await this.CacheRoomService.GetAll()).ToList();
            if (rooms != null)
            {
                rooms.ForEach(async (room) =>
                {
                    await this.SetRoomDetails(room);    
                });
            }
            return rooms;
        }

        public async Task<Room> GetRoom(string id)
        {
            Room room = (await this.CacheRoomService.Get((arg) => arg.Id == id));
            if (room != null)
            {
                await this.SetRoomDetails(room);
            }
            return room;
        }

        public async Task<IEnumerable<Room>> GetRooms(string locationId)
        {
            List<Room> rooms = (await this.CacheRoomService.GetAll((arg) => arg.LocationId == locationId)).ToList();
            if (rooms != null)
            {
                rooms.ForEach(async (room) =>
                {
                    await this.SetRoomDetails(room);
                });
            }
            return rooms;
        }
        public async Task<Room> GetRoomFromPlugId(string plugId)
        {
            Room room = null;
            Plug plug = await this.CachePlugService.Get((plug) => plug.Id == plugId);
            if (plug != null)
            {
                ConnectedObject connectedObject = await this.CacheConnectedObjectService.Get((obj) => obj.Id == plug.ConnectedObjectId);
                if (connectedObject != null)
                {
                    room = await this.CacheRoomService.Get((room) => room.Id == connectedObject.RoomId);
                }
            }
            return room;
        }
        public async Task<ResultCode> UpdateRoom(Room room)
        {
            ResultCode code = await this.Supervisor.UpdateRoom(room);
            if (code == ResultCode.Ok)
            {
                await this.CacheRoomService?.Set(room.Id, room);
            }
            return code;
        }
        public async Task<ResultCode> AddRoom(Room room)
        {
            ResultCode code = await this.Supervisor.AddRoom(room);
            if (code == ResultCode.Ok)
            {
                await this.CacheRoomService.Set(room.Id, room);
            }
            return code;
        }
        private async Task SetRoomDetails(Room room)
        {
            room.SensorsList = new ObservableCollection<Sensor>(await this.CacheSensorService.GetAll((arg) => arg.RoomId == room.Id));
            List<ConnectedObject> objects = (await this.GetConnectedObjectsFromCache((obj) => obj.RoomId == room.Id)).ToList();
            if (objects != null)
            {
                room.ConnectedObjectsList = objects;
                room.NotificationsList = (await this.CacheNotificationService.GetAll((notification) => notification.RoomId == room.Id)).ToList();
            }
            room.SetStatusSensors();

            await this.CacheRoomService?.Set(room.Id, room);
        }
        #endregion
    }
}

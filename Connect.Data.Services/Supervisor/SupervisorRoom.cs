using Connect.Data.Entities;
using Connect.Data.Mappers;
using Connect.Data.Services.Repositories;
using Connect.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorRoom : ISupervisorRoom
    {
        private readonly Lazy<IRoomRepository> _lazyRoomRepository;
        private readonly Lazy<IRepository<SensorEntity>> _lazySensorRepository;
        private readonly Lazy<IRepository<ProgramEntity>> _lazyProgramRepository;
        private readonly Lazy<IRepository<ConnectedObjectEntity>> _lazyConnectedObjectRepository;
        private readonly Lazy<IRepository<PlugEntity>> _lazyPlugRepository;
        private readonly Lazy<IRepository<NotificationEntity>> _lazyNotificationRepository;
        private readonly Lazy<IRepository<ConfigurationEntity>> _lazyConfigurationRepository;
        private readonly Lazy<IRepository<ConditionEntity>> _lazyConditionRepository;

        #region Properties
        private IRoomRepository RoomRepository => _lazyRoomRepository.Value;
        private IRepository<SensorEntity> SensorRepository => _lazySensorRepository.Value;
        private IRepository<ProgramEntity> ProgramRepository => _lazyProgramRepository.Value;
        private IRepository<ConnectedObjectEntity> ConnectedObjectRepository => _lazyConnectedObjectRepository.Value;
        private IRepository<PlugEntity> PlugRepository => _lazyPlugRepository.Value;
        private IRepository<NotificationEntity> NotificationRepository => _lazyNotificationRepository.Value;
        private IRepository<ConfigurationEntity> ConfigurationRepository => _lazyConfigurationRepository.Value;
        private IRepository<ConditionEntity> ConditionRepository => _lazyConditionRepository.Value; 
        #endregion

        #region Constructor
        public SupervisorRoom(IDataContextFactory dataContextFactory, IRepositoryFactory repositoryFactory, IConfiguration configuration)
        {
            ConnectionType type = new ConnectionType()
            {
                ConnectionString = configuration["ConnectionStrings:DefaultConnection"],
                ServerType = ConnectionType.GetServerType(configuration["ConnectionStrings:ServerType"]),
            };

            IDataContext? context = dataContextFactory.CreateDbContext(type.ConnectionString, type.ServerType)?.context;
            if (context != null)
            {
                _lazyRoomRepository = repositoryFactory?.CreateRoomRepository(context);
                _lazySensorRepository = repositoryFactory.CreateRepository<SensorEntity>(context);
                _lazyProgramRepository = repositoryFactory.CreateRepository<ProgramEntity>(context);
                _lazyConnectedObjectRepository = repositoryFactory.CreateRepository<ConnectedObjectEntity>(context);
                _lazyPlugRepository = repositoryFactory.CreateRepository<PlugEntity>(context);
                _lazyNotificationRepository = repositoryFactory.CreateRepository<NotificationEntity>(context); 
                _lazyConfigurationRepository = repositoryFactory.CreateRepository<ConfigurationEntity>(context);
                _lazyConditionRepository = repositoryFactory.CreateRepository<ConditionEntity>(context);
            }
        }
        #endregion
        
        #region Methods
        public async Task<ResultCode> RoomExists(string id)
        {
            return (await this.RoomRepository.GetAsync(id) != null) ? ResultCode.Ok : ResultCode.ItemNotFound;
        }

        public async Task<IEnumerable<Room>> GetRooms()
        {
            List<Room> rooms = null;
            IEnumerable<RoomEntity> entities = (await this.RoomRepository.GetCollectionAsync()).OrderBy((room) => room.Id);
            if (entities != null)
            {
                rooms = entities.Select(item => RoomMapper.Map(item)).ToList();                
                foreach (Room room in rooms)
                {
                    room.SetStatusSensors();
                }
            }
            return rooms;
        }

        public async Task<IEnumerable<Room>> GetRooms(string locationId)
		{
            List<Room> rooms = null;
            IEnumerable<RoomEntity> entities = await this.RoomRepository.GetCollectionAsync((room) => room.LocationId == locationId);
            if (entities != null)
			{
                rooms = entities.Select(item => RoomMapper.Map(item)).ToList();
                for (int i = 0; i < rooms.Count(); i++)
				{
                    rooms[i] = await this.GetRoomDetails(rooms[i]);
                }
            }
            return rooms;
        }

        public async Task<Room> GetRoom(string? id)
        {
            Room room = RoomMapper.Map(await this.RoomRepository.GetAsync(id));
            if (room != null)
            {
                room = await this.GetRoomDetails(room);
            }
            return room;
        }

        private async Task<Room> GetRoomDetails(Room room)
        {
            IEnumerable<SensorEntity> sensorEntities = await this.SensorRepository.GetCollectionAsync((sensor) => sensor.RoomId == room.Id);
            if (sensorEntities != null)
            {
                room.SensorsList = new ObservableCollection<Sensor>(sensorEntities.Select(item => SensorMapper.Map(item)));
            }

            IEnumerable<ConnectedObjectEntity> objectEntities = await this.ConnectedObjectRepository.GetCollectionAsync((obj) => obj.RoomId == room.Id);
            if (objectEntities != null)
            {
                List<ConnectedObject> objects = objectEntities.Select(item => ConnectedObjectMapper.Map(item)).ToList();
                foreach (ConnectedObject obj in objects)
                {
                    IEnumerable<NotificationEntity> notificationEntities = (await this.NotificationRepository.GetCollectionAsync((notification) => notification.ConnectedObjectId == obj.Id));
                    if (notificationEntities != null)
                    {
                        obj.NotificationsList = notificationEntities.Select(item => NotificationMapper.Map(item)).ToList();
                    }

                    PlugEntity plugEntity = await this.PlugRepository.GetAsync((plug) => plug.ConnectedObjectId == obj.Id);
                    if (plugEntity != null)
                    {
                        Plug plug = PlugMapper.Map(plugEntity);
                        plug.Configuration = ConfigurationMapper.Map(await this.ConfigurationRepository.GetAsync((arg) => arg.Id == plug.ConfigurationId));
                        plug.Program = ProgramMapper.Map(await this.ProgramRepository.GetAsync((arg) => arg.Id == plug.ProgramId));
                        plug.Condition = ConditionMapper.Map(await this.ConditionRepository.GetAsync((arg) => arg.Id == plug.ConditionId));
                        obj.Plug = plug;
                    }

                    SensorEntity sensorEntity = await this.SensorRepository.GetAsync((sensor) => sensor.ConnectedObjectId == obj.Id);
                    if (sensorEntity != null)
                    {
                        obj.Sensor = SensorMapper.Map(sensorEntity);
                    }

                    room.ConnectedObjectsList.Add(obj);

                    notificationEntities = null;
                    notificationEntities = (await this.NotificationRepository.GetCollectionAsync((notification) => notification.RoomId == room.Id));
                    if (notificationEntities != null)
                    {
                        room.NotificationsList = notificationEntities.Select(item => NotificationMapper.Map(item)).ToList();
                    }
                }
            }

            room.SetStatusSensors();

            return room;
        }

        public async Task<Room> GetRoomFromPlugId(string plugId)
        {
            return RoomMapper.Map(await this.RoomRepository.GetFromPlugIdAsync(plugId));
        }

        public async Task<ResultCode> UpdateRoom(Room room)
        {
            ResultCode result = await this.RoomExists(room?.Id);
            if (result == ResultCode.Ok)
            {
                result =  (await this.RoomRepository.UpdateAsync(RoomMapper.Map(room)) > 0) ? ResultCode.Ok : ResultCode.CouldNotUpdateItem;
            }
            return (result);
        }

        public async Task<ResultCode> AddRoom(Room room)
        {
            room.Id = string.IsNullOrEmpty(room.Id) ? Guid.NewGuid().ToString() : room.Id;
            int res = await this.RoomRepository.InsertAsync(RoomMapper.Map(room));
            ResultCode result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
            return result;
        }
        #endregion
    }
}

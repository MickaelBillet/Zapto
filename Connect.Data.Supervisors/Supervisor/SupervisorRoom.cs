using Connect.Data.Entities;
using Connect.Data.Mappers;
using Connect.Data.Services.Repositories;
using Connect.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using System.Collections.ObjectModel;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorRoom : Supervisor, ISupervisorRoom
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
        public SupervisorRoom(IDalSession session, IRepositoryFactory repositoryFactory)
        {
            _lazyRoomRepository = repositoryFactory?.CreateRoomRepository(session);
            _lazySensorRepository = repositoryFactory.CreateRepository<SensorEntity>(session);
            _lazyProgramRepository = repositoryFactory.CreateRepository<ProgramEntity>(session);
            _lazyConnectedObjectRepository = repositoryFactory.CreateRepository<ConnectedObjectEntity>(session);
            _lazyPlugRepository = repositoryFactory.CreateRepository<PlugEntity>(session);
            _lazyNotificationRepository = repositoryFactory.CreateRepository<NotificationEntity>(session);
            _lazyConfigurationRepository = repositoryFactory.CreateRepository<ConfigurationEntity>(session);
            _lazyConditionRepository = repositoryFactory.CreateRepository<ConditionEntity>(session);
        }
        #endregion
        
        #region Methods
        public async Task<ResultCode> RoomExists(string id)
        {
            return (await this.RoomRepository.GetAsync(id) != null) ? ResultCode.Ok : ResultCode.ItemNotFound;
        }

        public async Task<IEnumerable<Room>> GetRooms()
        {
            IEnumerable<Room> rooms = null;
            IEnumerable<RoomEntity> entities = (await this.RoomRepository.GetCollectionAsync()).OrderBy((room) => room.Id);
            if (entities != null)
            {
                rooms = entities.Select(item => RoomMapper.Map(item));
                foreach (Room room in rooms)
                {
                    await this.SetRoomDetails(room);
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
                foreach(Room room in rooms)
				{
                    await this.SetRoomDetails(room);
                }
            }
            return rooms;
        }

        public async Task<Room> GetRoom(string id)
        {
            Room room = RoomMapper.Map(await this.RoomRepository.GetAsync(id));
            if (room != null)
            {
                await this.SetRoomDetails(room);
            }
            return room;
        }

        private async Task<ResultCode> SetRoomDetails(Room room)
        {
            ResultCode resultCode = ResultCode.Ok;
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
                    await this.SetConnectedObjectDetails(obj);

                    room.ConnectedObjectsList.Add(obj);

                    IEnumerable<NotificationEntity> notificationEntities = (await this.NotificationRepository.GetCollectionAsync((notification) => notification.RoomId == room.Id));
                    if (notificationEntities != null)
                    {
                        room.NotificationsList = notificationEntities.Select(item => NotificationMapper.Map(item)).ToList();
                    }
                }
            }

            room.SetStatusSensors();

            resultCode = (await this.RoomRepository.UpdateAsync(RoomMapper.Map(room)) > 0) ? ResultCode.Ok : ResultCode.CouldNotUpdateItem;

            return resultCode;
        }

        private async Task SetConnectedObjectDetails(ConnectedObject connectedObject)
        {
            IEnumerable<NotificationEntity> notificationEntities = (await this.NotificationRepository.GetCollectionAsync((notification) => notification.ConnectedObjectId == connectedObject.Id));
            if (notificationEntities != null)
            {
                connectedObject.NotificationsList = notificationEntities.Select(item => NotificationMapper.Map(item)).ToList();
            }

            PlugEntity plugEntity = await this.PlugRepository.GetAsync((plug) => plug.ConnectedObjectId == connectedObject.Id);
            if (plugEntity != null)
            {
                Plug plug = PlugMapper.Map(plugEntity);
                plug.Configuration = ConfigurationMapper.Map(await this.ConfigurationRepository.GetAsync((arg) => arg.Id == plug.ConfigurationId));
                plug.Program = ProgramMapper.Map(await this.ProgramRepository.GetAsync((arg) => arg.Id == plug.ProgramId));
                plug.Condition = ConditionMapper.Map(await this.ConditionRepository.GetAsync((arg) => arg.Id == plug.ConditionId));
                connectedObject.Plug = plug;
            }

            SensorEntity sensorEntity = await this.SensorRepository.GetAsync((sensor) => sensor.ConnectedObjectId == connectedObject.Id);
            if (sensorEntity != null)
            {
                connectedObject.Sensor = SensorMapper.Map(sensorEntity);
            }
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

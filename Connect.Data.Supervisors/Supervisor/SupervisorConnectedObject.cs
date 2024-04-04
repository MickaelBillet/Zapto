using Connect.Data.Entities;
using Connect.Data.Mappers;
using Connect.Data.Services.Repositories;
using Connect.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using System.Collections.ObjectModel;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorConnectedObject : ISupervisorConnectedObject
    {
        private readonly Lazy<IRepository<ConditionEntity>> _lazyConditionRepository;
        private readonly Lazy<IRepository<OperationRangeEntity>> _lazyOperationRangeRepository;
        private readonly Lazy<IRepository<SensorEntity>> _lazySensorRepository;
        private readonly Lazy<IRepository<ProgramEntity>> _lazyProgramRepository;
        private readonly Lazy<IRepository<PlugEntity>> _lazyPlugRepository;
        private readonly Lazy<IRepository<ConfigurationEntity>> _lazyConfigurationRepository;
        private readonly Lazy<IRepository<NotificationEntity>> _lazyNotificationRepository;
        private readonly Lazy<IRepository<ConnectedObjectEntity>> _lazyConnectedObjectRepository;

        #region Properties
        private IRepository<ConditionEntity> ConditionRepository => _lazyConditionRepository.Value;
        private IRepository<OperationRangeEntity> OperationRangeRepository => _lazyOperationRangeRepository.Value;
        private IRepository<SensorEntity> SensorRepository => _lazySensorRepository.Value;
        private IRepository<ProgramEntity> ProgramRepository => _lazyProgramRepository.Value;
        private IRepository<PlugEntity> PlugRepository => _lazyPlugRepository.Value;
        private IRepository<ConfigurationEntity> ConfigurationRepository => _lazyConfigurationRepository.Value;
        private IRepository<NotificationEntity> NotificationRepository => _lazyNotificationRepository.Value;
        private IRepository<ConnectedObjectEntity> ConnectedObjectRepository => _lazyConnectedObjectRepository.Value;
        #endregion

        #region Constructor
        public SupervisorConnectedObject(IDalSession session, IRepositoryFactory repositoryFactory)
        {
            _lazyConditionRepository = repositoryFactory.CreateRepository<ConditionEntity>(session);
            _lazyOperationRangeRepository = repositoryFactory.CreateRepository<OperationRangeEntity>(session);
            _lazySensorRepository = repositoryFactory.CreateRepository<SensorEntity>(session);
            _lazyPlugRepository = repositoryFactory.CreateRepository<PlugEntity>(session);
            _lazyProgramRepository = repositoryFactory.CreateRepository<ProgramEntity>(session);
            _lazyConfigurationRepository = repositoryFactory.CreateRepository<ConfigurationEntity>(session);
            _lazyNotificationRepository = repositoryFactory.CreateRepository<NotificationEntity>(session);
            _lazyConnectedObjectRepository = repositoryFactory.CreateRepository<ConnectedObjectEntity>(session);
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<ConnectedObject>> GetConnectedObjects()
        {
            List<ConnectedObject> objs = null;
            IEnumerable<ConnectedObjectEntity> entities = await this.ConnectedObjectRepository.GetCollectionAsync();
            if (entities != null)
            {
                objs = entities.Select(item => ConnectedObjectMapper.Map(item)).ToList();
                foreach (ConnectedObject obj in objs)
                {
                    PlugEntity plugEntity = await this.PlugRepository.GetAsync((plug) => plug.ConnectedObjectId == obj.Id);
                    if (plugEntity != null)
                    {
                        obj.Plug = PlugMapper.Map(plugEntity);
                    }
                    SensorEntity sensorEntity = await this.SensorRepository.GetAsync((sensor) => sensor.ConnectedObjectId == obj.Id);
                    if (sensorEntity != null) 
                    {
                        obj.Sensor = SensorMapper.Map(sensorEntity);
                    }
                    IEnumerable<NotificationEntity> notificationEntities = (await this.NotificationRepository.GetCollectionAsync((notification) => notification.ConnectedObjectId == obj.Id));
                    if (notificationEntities != null)
                    {
                        obj.NotificationsList = notificationEntities.Select(item => NotificationMapper.Map(item)).ToList();
                    }
                }
            }

            return objs;
        }

        public async Task<ResultCode> ConnectedObjectExists(string id)
        {
            return (await this.ConnectedObjectRepository.GetAsync(id) != null) ? ResultCode.Ok : ResultCode.ItemNotFound;
        }

        public async Task<ConnectedObject> GetConnectedObject(string id)
        {
            ConnectedObject obj = ConnectedObjectMapper.Map(await this.ConnectedObjectRepository.GetAsync(id));
            if (obj != null) 
            {
                PlugEntity plugEntity = await this.PlugRepository.GetAsync((plug) => plug.ConnectedObjectId == id);
                if (plugEntity != null)
                {
                    obj.Plug = PlugMapper.Map(plugEntity);
                    obj.Plug.Configuration = ConfigurationMapper.Map(await this.ConfigurationRepository.GetAsync((arg) => arg.Id == obj.Plug.ConfigurationId));
                    obj.Plug.Program = ProgramMapper.Map(await this.ProgramRepository.GetAsync((arg) => arg.Id == obj.Plug.ProgramId));
                    obj.Plug.Condition = ConditionMapper.Map(await this.ConditionRepository.GetAsync((arg) => arg.Id == obj.Plug.ConditionId));

                    if (obj.Plug.Program != null)
                    {
                        //Read the operation ranges of the program
                        IEnumerable<OperationRangeEntity> operationRangeEntities = await this.OperationRangeRepository.GetCollectionAsync((arg) => arg.ProgramId == obj.Plug.ProgramId);
                        if (operationRangeEntities != null)
                        {
                            List<OperationRange> operationRanges = operationRangeEntities.Select(item => OperationRangeMapper.Map(item)).ToList();
                            foreach (OperationRange operationRange in operationRanges)
                            {
                                operationRange.Condition = ConditionMapper.Map(await this.ConditionRepository.GetAsync((arg) => arg.Id == operationRange.ConditionId));
                            }

                            obj.Plug.Program.OperationRangeList = new ObservableCollection<OperationRange>(operationRanges);
                        }
                    }
                }

                SensorEntity sensorEntity = await this.SensorRepository.GetAsync((sensor) => sensor.ConnectedObjectId == id);
                if (sensorEntity != null)
                {
                    obj.Sensor = SensorMapper.Map(sensorEntity);
                }

                IEnumerable<NotificationEntity> notificationEntities = (await this.NotificationRepository.GetCollectionAsync((notification) => notification.ConnectedObjectId == id));
                if (notificationEntities != null)
                {
                    obj.NotificationsList = notificationEntities.Select(item => NotificationMapper.Map(item)).ToList();
                }
            }

            return obj;
        }
        public async Task<ResultCode> AddConnectedObject(ConnectedObject obj)
        {
            obj.Id = string.IsNullOrEmpty(obj.Id) ? Guid.NewGuid().ToString() : obj.Id;
            int res = await this.ConnectedObjectRepository.InsertAsync(ConnectedObjectMapper.Map(obj));
            ResultCode result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
            return result;
        }
        #endregion
    }
}

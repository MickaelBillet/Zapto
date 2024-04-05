using Connect.Model;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Connect.Data.Supervisors
{
    public abstract class SupervisorCache
    {
        #region Services
        protected ICacheZaptoService<Room> CacheRoomService { get; }
        protected ICacheZaptoService<Sensor> CacheSensorService { get; }
        protected ICacheZaptoService<ConnectedObject> CacheConnectedObjectService { get; }
        protected ICacheZaptoService<Notification> CacheNotificationService { get; }
        protected ICacheZaptoService<Configuration> CacheConfigurationService { get; }
        protected ICacheZaptoService<Program> CacheProgramService { get; }
        protected ICacheZaptoService<Condition> CacheConditionService { get; }
        protected ICacheZaptoService<Plug> CachePlugService { get; }
        protected ICacheZaptoService<OperationRange> CacheOperationRangeService { get; }
        protected ICacheZaptoService<Location> CacheLocationService { get; }
        #endregion

        #region Constructor
        public SupervisorCache(IServiceProvider serviceProvider)
        {
            this.CacheRoomService = serviceProvider.GetRequiredService<ICacheZaptoService<Room>>();
            this.CacheSensorService = serviceProvider.GetRequiredService<ICacheZaptoService<Sensor>>();
            this.CacheConnectedObjectService = serviceProvider.GetRequiredService<ICacheZaptoService<ConnectedObject>>();
            this.CacheNotificationService = serviceProvider.GetRequiredService<ICacheZaptoService<Notification>>();
            this.CacheConfigurationService = serviceProvider.GetRequiredService<ICacheZaptoService<Configuration>>();
            this.CacheProgramService = serviceProvider.GetRequiredService<ICacheZaptoService<Program>>();
            this.CacheConditionService = serviceProvider.GetRequiredService<ICacheZaptoService<Condition>>();
            this.CachePlugService = serviceProvider.GetRequiredService<ICacheZaptoService<Plug>>();
            this.CacheOperationRangeService = serviceProvider.GetRequiredService<ICacheZaptoService<OperationRange>>();
            this.CacheLocationService = serviceProvider.GetRequiredService<ICacheZaptoService<Location>>();
        }
        #endregion

        #region Method
        public abstract Task Initialize();

        protected async Task<Plug> GetPlugFromCache(Func<Plug, bool> func)
        {
            Plug plug = await this.CachePlugService.Get(func);
            if (plug != null)
            {
                plug.Configuration = await this.CacheConfigurationService.Get((arg) => arg.Id == plug.ConfigurationId);
                plug.Program = await this.CacheProgramService.Get((arg) => arg.Id == plug.ProgramId);
                plug.Condition = await this.CacheConditionService.Get((arg) => arg.Id == plug.ConditionId);
                if (plug.Program != null)
                {
                    List<OperationRange> operationRanges = (await this.CacheOperationRangeService.GetAll((arg) => arg.ProgramId == plug.ProgramId)).ToList();
                    if (operationRanges != null)
                    {
                        foreach (OperationRange operationRange in operationRanges)
                        {
                            operationRange.Condition = await this.CacheConditionService.Get((arg) => arg.Id == operationRange.Id);
                        }
                        plug.Program.OperationRangeList = new ObservableCollection<OperationRange>(operationRanges);
                    }
                }
            }
            return plug;
        }

        protected async Task<IEnumerable<ConnectedObject>> GetConnectedObjectsFromCache(Func<ConnectedObject, bool> func)
        {
            List<ConnectedObject> connectedObjects = (await this.CacheConnectedObjectService.GetAll(func)).ToList();
            if (connectedObjects != null) 
            {
                connectedObjects.ForEach(async (connectedObject) =>
                {
                    connectedObject.NotificationsList = (await this.CacheNotificationService.GetAll((arg) => arg.ConnectedObjectId == connectedObject.Id)).ToList();
                    connectedObject.Plug = await this.GetPlugFromCache((plug) => plug.ConnectedObjectId == connectedObject.Id);
                    connectedObject.Sensor = await this.CacheSensorService.Get((sensor) => sensor.ConnectedObjectId == connectedObject.Id);
                });
            }
            return connectedObjects;
        }
        #endregion
    }
}

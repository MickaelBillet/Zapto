using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public class SupervisorCacheConnectedObject : SupervisorCache, ISupervisorConnectedObject
    {
        #region Services
        private ISupervisorConnectedObject Supervisor { get; }
        #endregion

        #region Constructor
        public SupervisorCacheConnectedObject(IServiceProvider serviceProvider) : base(serviceProvider) 
        {
            this.Supervisor = serviceProvider.GetRequiredService<ISupervisorFactoryConnectedObject>().CreateSupervisor(0);
        }
        #endregion

        #region Methods
        public async Task<ConnectedObject> GetConnectedObject(string id)
        {
            ConnectedObject connectedObject = await this.CacheConnectedObjectService.Get((arg) =>  arg.Id == id);
            if (connectedObject != null)
            {
                connectedObject.Plug = await this.GetPlugFromCache((plug) => plug.ConnectedObjectId == id);
                connectedObject.Sensor = await this.CacheSensorService.Get((sensor) => sensor.ConnectedObjectId == id);
                connectedObject.NotificationsList = (await this.CacheNotificationService.GetAll((notification) => notification.ConnectedObjectId == id)).ToList();
            }
            return connectedObject;
        }
        public async Task<IEnumerable<ConnectedObject>> GetConnectedObjects()
        {
            return await this.GetConnectedObjectsFromCache(null);
        }
        public async Task Initialize()
        {
            IEnumerable<ConnectedObject> connectedObjects = await this.Supervisor.GetConnectedObjects();
            foreach (var item in connectedObjects)
            {
                await this.CacheConnectedObjectService.Set(item.Id, item);
            }
        }

        public async Task<ResultCode> AddConnectedObject(ConnectedObject obj)
        {
            ResultCode code = await this.Supervisor.AddConnectedObject(obj);
            if (code == ResultCode.Ok)
            {
                await this.CacheConnectedObjectService.Set(obj.Id, obj);
            }
            return code;
        }
        #endregion
    }
}

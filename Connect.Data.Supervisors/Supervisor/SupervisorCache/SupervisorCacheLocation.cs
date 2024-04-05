using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.Data.Supervisors
{
    public class SupervisorCacheLocation : SupervisorCache, ISupervisorCacheLocation
    {
        #region Services
        private ISupervisorLocation Supervisor { get; }
        #endregion

        #region Constructor
        public SupervisorCacheLocation(IServiceProvider serviceProvider) : base (serviceProvider)
        {
            this.Supervisor = serviceProvider.GetRequiredService<ISupervisorLocation>();
        }
        #endregion

        #region Methods
        public override async Task Initialize()
        {
            IEnumerable<Location> locations = await this.Supervisor.GetLocations();
            foreach (var item in locations)
            {
                await this.CacheLocationService.Set(item.Id, item);
            }
        }
        public async Task<IEnumerable<Location>> GetLocations()
        {
            return await this.CacheLocationService.GetAll();
        }
        public async Task<Location> GetLocation(string id)
        {
            Location location = await this.CacheLocationService.Get((arg) => arg.Id == id);
            if (location != null)
            {
                location.RoomsList = await this.CacheRoomService.GetAll((arg) => arg.LocationId == id);
            }
            return location;
        }
        public async Task<ResultCode> AddLocation(Location location)
        {
            ResultCode code = await this.Supervisor.AddLocation(location);
            if (code == ResultCode.Ok)
            {
                await this.CacheLocationService.Set(location.Id, location);
            }
            return code;
        }
        #endregion
    }
}

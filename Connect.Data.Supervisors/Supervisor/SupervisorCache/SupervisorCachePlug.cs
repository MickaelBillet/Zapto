using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorCachePlug : SupervisorCache, ISupervisorCachePlug
    {
        #region Services
        private ISupervisorPlug Supervisor { get; }
        #endregion

        #region Constructor
        public SupervisorCachePlug(IServiceProvider serviceProvider) : base(serviceProvider) 
        {
            this.Supervisor = serviceProvider.GetRequiredService<ISupervisorPlug>();
        }
        #endregion

        #region Methods
        public async Task<ResultCode> AddPlug(Plug plug)
        {
            ResultCode code = await this.Supervisor.AddPlug(plug);
            if (code == ResultCode.Ok) 
            {
                await this.CachePlugService.Set(plug.Id, plug);
            }
            return code;
        }
        public override async Task Initialize()
        {
            IEnumerable<Plug> plugs = await this.Supervisor.GetPlugs();
            foreach (var item in plugs)
            {
                await this.CachePlugService.Set(item.Id, item);    
            }            
        }
        public async Task<IEnumerable<Plug>> GetPlugs()
        {
            IEnumerable<Plug> plugs = await this.GetPlugsFromCache();
            return plugs;
        }
        public async Task<Plug> GetPlug(string id)
        {
            Plug plug = await this.GetPlugFromCache((arg) => arg.Id == id);
            return plug;
        }
        public async Task<ResultCode> UpdatePlug(Plug plug)
        {
            ResultCode code = await this.Supervisor.UpdatePlug(plug);
            if (code == ResultCode.Ok)
            {
                await this.CachePlugService?.Set(plug.Id, plug);
            }
            return code;
        }
        public async Task<Plug> GetPlug(string address, string unit)
        {
            Plug plug = await this.GetPlugFromCache(address, unit);
            return plug;
        }
        public async Task<ResultCode> ResetWorkingDuration(Plug plug)
        {
            ResultCode code = ResultCode.Ok;
            if (plug != null)
            {
                plug.WorkingDuration = 0;
                code = await this.UpdatePlug(plug);
            }
            return code;
        }        
        private async Task<IEnumerable<Plug>> GetPlugsFromCache()
        {
            List<Plug> plugs = (await this.CachePlugService.GetAll()).ToList();
            if (plugs != null)
            {
                plugs.ForEach(async (plug) =>
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
                });
            }
            return plugs;
        }
        private async Task<Plug> GetPlugFromCache(string address, string unit)
        {
            Plug plug = null;
            Configuration config = await this.CacheConfigurationService.Get(arg => ((arg.Address == address) && (arg.Unit == unit)));
            if (config != null) 
            {
                plug = await this.CachePlugService.Get(arg => ((arg.ConfigurationId == config.Id)));
            }
            return plug;
        }
        #endregion
    }
}

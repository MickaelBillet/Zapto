using Connect.Data.Entities;
using Connect.Data.Mappers;
using Connect.Data.Services.Repositories;
using Connect.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using System.Collections.ObjectModel;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorPlug : ISupervisorPlug
    {
        private readonly Lazy<IPlugRepository> _lazyPlugRepository;
        private readonly Lazy<IRepository<ConfigurationEntity>> _lazyConfigurationRepository;
        private readonly Lazy<IRepository<ConditionEntity>> _lazyConditionRepository;
        private readonly Lazy<IRepository<OperationRangeEntity>> _lazyOperationRangeRepository;
        private readonly Lazy<IRepository<ProgramEntity>> _lazyProgramRepository;

        #region Properties
        private IPlugRepository PlugRepository => _lazyPlugRepository.Value;
        private IRepository<ConfigurationEntity> ConfigurationRepository => _lazyConfigurationRepository.Value;
        private IRepository<ConditionEntity> ConditionRepository => _lazyConditionRepository.Value;
        private IRepository<OperationRangeEntity> OperationRangeRepository => _lazyOperationRangeRepository.Value;
        private IRepository<ProgramEntity> ProgramRepository => _lazyProgramRepository.Value;
        #endregion

        #region Constructor
        public SupervisorPlug(IDalSession session, IRepositoryFactory repositoryFactory)
        {
            _lazyPlugRepository = repositoryFactory.CreatePlugRepository(session);
            _lazyConfigurationRepository = repositoryFactory?.CreateRepository<ConfigurationEntity>(session);
            _lazyConditionRepository = repositoryFactory?.CreateRepository<ConditionEntity>(session);
            _lazyOperationRangeRepository = repositoryFactory.CreateRepository<OperationRangeEntity>(session);
            _lazyProgramRepository = repositoryFactory.CreateRepository<ProgramEntity>(session);
        }
        #endregion

        #region Methods
        public async Task<ResultCode> Upgrade1_1()
        {
            return (await this.PlugRepository.Upgrade1_1() == 0) ? ResultCode.Ok : ResultCode.CouldNotUpdateItem;
        }

        public async Task<ResultCode> PlugExists(string id)
        {
            return (await this.PlugRepository.GetAsync(id) != null) ? ResultCode.Ok : ResultCode.ItemNotFound;
        }

        public async Task<IEnumerable<Plug>> GetPlugs()
        {
            List<Plug> plugs = null;
            IEnumerable<PlugEntity> entities = (await this.PlugRepository.GetCollectionAsync()).OrderBy((plug) => plug.Id);
            if (entities != null)
            {
                plugs = entities.Select(item => PlugMapper.Map(item)).ToList();
                foreach (Plug plug in plugs)
                {
                    plug.Configuration =  ConfigurationMapper.Map(await this.ConfigurationRepository.GetAsync((arg) => arg.Id == plug.ConfigurationId));
                    plug.Program = ProgramMapper.Map(await this.ProgramRepository.GetAsync((arg) => arg.Id == plug.ProgramId));
                    plug.Condition = ConditionMapper.Map(await this.ConditionRepository.GetAsync((arg) => arg.Id == plug.ConditionId));
                    if (plug.Program != null)
                    {
                        //Read the operation ranges of the program
                        IEnumerable<OperationRangeEntity> operationRangeEntities = await this.OperationRangeRepository.GetCollectionAsync((arg) => arg.ProgramId == plug.ProgramId);
                        if (operationRangeEntities != null)
                        {
                            List<OperationRange> operationRanges = operationRangeEntities.Select(item => OperationRangeMapper.Map(item)).ToList();
                            foreach (OperationRange operationRange in operationRanges)
                            {
                                operationRange.Condition = ConditionMapper.Map(await this.ConditionRepository.GetAsync((arg) => arg.Id == operationRange.ConditionId));
                            }
                            plug.Program.OperationRangeList = new ObservableCollection<OperationRange>(operationRanges);
                        }
                    }
                }
            }
            return plugs;
        }

        public async Task<Plug> GetPlug(string id)
        {
            Plug plug = null;
            PlugEntity entity = await this.PlugRepository.GetAsync(id);
            if (entity != null)
            {
                plug = PlugMapper.Map(entity);
                plug.Configuration = ConfigurationMapper.Map(await this.ConfigurationRepository.GetAsync((arg) => arg.Id == plug.ConfigurationId));
                plug.Program = ProgramMapper.Map(await this.ProgramRepository.GetAsync((arg) => arg.Id == plug.ProgramId));
                plug.Condition = ConditionMapper.Map(await this.ConditionRepository.GetAsync((arg) => arg.Id == plug.ConditionId));
                if (plug.Program != null)
                {
                    //Read the operation ranges of the program
                    IEnumerable<OperationRangeEntity> operationRangeEntities = await this.OperationRangeRepository.GetCollectionAsync((arg) => arg.ProgramId == plug.ProgramId);
                    if (operationRangeEntities != null)
                    {
                        List<OperationRange> operationRanges = operationRangeEntities.Select(item => OperationRangeMapper.Map(item)).ToList();
                        foreach (OperationRange operationRange in operationRanges)
                        {
                            operationRange.Condition = ConditionMapper.Map(await this.ConditionRepository.GetAsync((arg) => arg.Id == operationRange.ConditionId));
                        }
                        plug.Program.OperationRangeList = new ObservableCollection<OperationRange>(operationRanges);
                    }
                }
            }

            return plug;
        }

        public async Task<ResultCode> AddPlug(Plug plug)
        {
            plug.Id = string.IsNullOrEmpty(plug.Id) ? Guid.NewGuid().ToString() : plug.Id;
            int res = await this.PlugRepository.InsertAsync(PlugMapper.Map(plug));
            ResultCode result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
            return result;
        }

        public async Task<ResultCode> UpdatePlug(Plug plug)
        {
            ResultCode result = await this.PlugExists(plug?.Id);

            if (result == ResultCode.Ok)
            {
                int res = await this.PlugRepository.UpdateAsync(PlugMapper.Map(plug));
                result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotUpdateItem;
            }

            return result;
        }

        public async Task<Plug> GetPlug(string address, string unit)
        {
            Plug plug = null;
            Configuration config = ConfigurationMapper.Map(await this.ConfigurationRepository.GetAsync(arg => ((arg.Address == address) && (arg.Unit == unit))));
            if (config != null)
            {
                //Test if the item exists - the configuration is unique for a plug
                plug = PlugMapper.Map(await this.PlugRepository.GetAsync(arg => ((arg.ConfigurationId == config.Id))));                
            }
            return plug;
        }

        public async Task<ResultCode> ResetWorkingDuration(Plug plug)
        {
            ResultCode result = ResultCode.Ok;

            if (plug != null)
            {
                plug.WorkingDuration = 0;

                //Update the data in the database
                result = (await this.PlugRepository.UpdateAsync(PlugMapper.Map(plug)) > 0) ? ResultCode.Ok : ResultCode.CouldNotUpdateItem;
            }
            else
            {
                result = ResultCode.ItemNotFound;
            }

            return result;
        }
        #endregion
    }
}

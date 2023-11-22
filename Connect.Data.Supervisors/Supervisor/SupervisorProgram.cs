using Connect.Data.Entities;
using Connect.Data.Mappers;
using Connect.Data.Services.Repositories;
using Connect.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using System.Collections.ObjectModel;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorProgram : ISupervisorProgram
    {
        private readonly Lazy<IRepository<ConditionEntity>> _lazyConditionRepository;
        private readonly Lazy<IRepository<OperationRangeEntity>> _lazyOperationRangeRepository;
        private readonly Lazy<IRepository<ProgramEntity>> _lazyProgramRepository;
        private readonly Lazy<IRepository<ConnectedObjectEntity>> _lazyConnectedObjectRepository;

        #region Properties
        private IRepository<ConditionEntity> ConditionRepository => _lazyConditionRepository.Value;
        private IRepository<OperationRangeEntity> OperationRangeRepository => _lazyOperationRangeRepository.Value;
        private IRepository<ProgramEntity> ProgramRepository => _lazyProgramRepository.Value;
        private IRepository<ConnectedObjectEntity> ConnectedObjectRepository => _lazyConnectedObjectRepository.Value;
        #endregion

        #region Constructor
        public SupervisorProgram(IDalSession session, IRepositoryFactory repositoryFactory)
        {
            _lazyConditionRepository = repositoryFactory?.CreateRepository<ConditionEntity>(session);
            _lazyOperationRangeRepository = repositoryFactory.CreateRepository<OperationRangeEntity>(session);
            _lazyProgramRepository = repositoryFactory.CreateRepository<ProgramEntity>(session);
            _lazyConnectedObjectRepository = repositoryFactory.CreateRepository<ConnectedObjectEntity>(session);
        }
        #endregion

        #region Methods
        public async Task<ResultCode> ProgramExists(string id)
        {
            return (await this.ProgramRepository.GetAsync(id) != null) ? ResultCode.Ok : ResultCode.ItemNotFound;
        }

        public async Task<IEnumerable<Program>> GetPrograms()
        {
            IEnumerable<ProgramEntity> entities = await this.ProgramRepository.GetCollectionAsync();
            return entities.Select(item => ProgramMapper.Map(item));
        }

        public async Task<Program> GetProgram(string id)
        {
            Program program = null;
            ProgramEntity entity = await this.ProgramRepository.GetAsync(id);
            if (entity != null)
            {
                program = ProgramMapper.Map(entity);
                IEnumerable<OperationRangeEntity> entities = await this.OperationRangeRepository.GetCollectionAsync((operationRange) => operationRange.ProgramId == id);
                if (entities != null)
                {
                    List<OperationRange> operationRanges = entities.Select(item => OperationRangeMapper.Map(item)).ToList();
                    if (operationRanges != null)
                    {
                        foreach (OperationRange operationRange in operationRanges)
                        {
                            operationRange.Condition = ConditionMapper.Map(await this.ConditionRepository.GetAsync((condition) => condition.OperationRangetId == operationRange.Id));
                        }
                        program.OperationRangeList = new ObservableCollection<OperationRange>(operationRanges);
                    }
                }
            }

            return program;
        }

        public async Task<ResultCode> AddProgram(Program program)
        {
            program.Id = string.IsNullOrEmpty(program.Id) ? Guid.NewGuid().ToString() : program.Id;
            int res = await this.ProgramRepository.InsertAsync(ProgramMapper.Map(program));
            ResultCode result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
            return result;
        }
        #endregion
    }
}

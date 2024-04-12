using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorCacheProgram : SupervisorCache, ISupervisorProgram
    {
        #region Services
        private ISupervisorProgram Supervisor { get; }
        #endregion

        #region Constructor
        public SupervisorCacheProgram(IServiceProvider serviceProvider) : base(serviceProvider) 
        {
            this.Supervisor = serviceProvider.GetRequiredService<ISupervisorFactoryProgram>().CreateSupervisor(CacheType.None);
        }
        #endregion

        #region Methods
        public async Task Initialize()
        {
            IEnumerable<Program> programs = await this.Supervisor.GetPrograms();
            foreach (var item in programs)
            {
                await this.CacheProgramService.Set(item.Id, item);
            }
        }
        public async Task<ResultCode> AddProgram(Program program)
        {
            ResultCode code = await this.Supervisor.AddProgram(program);
            if (code == ResultCode.Ok)
            {
                await this.CacheProgramService.Set(program.Id, program);
            }
            return code;
        }
        public async Task<IEnumerable<Program>> GetPrograms()
        {
            return await this.Supervisor.GetPrograms();
        }
        public async Task<Program> GetProgram(string id)
        {
            Program program = await this.CacheProgramService.Get((arg) => arg.Id == id);
            if (program != null)
            {
                await this.SetProgramDetails(program);
            }
            return program;
        }
        private async Task SetProgramDetails(Program program)
        {
            List<OperationRange> operationRanges = (await this.CacheOperationRangeService.GetAll((arg) => arg.ProgramId == program.Id)).ToList();
            operationRanges.ForEach(async operationRange => 
            {
                operationRange.Condition = await this.CacheConditionService.Get((arg) => arg.OperationRangetId == operationRange.Id);
            });
            program.OperationRangeList = new ObservableCollection<OperationRange>(operationRanges);
        }

        #endregion
    }
}

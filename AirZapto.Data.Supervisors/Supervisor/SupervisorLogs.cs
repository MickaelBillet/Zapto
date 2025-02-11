using AirZapto.Data.Entities;
using AirZapto.Data.Mappers;
using AirZapto.Data.Services;
using AirZapto.Data.Services.Repositories;
using Framework.Core.Base;
using Framework.Core.Domain;
using Framework.Data.Abstractions;

namespace AirZapto.Data.Supervisors
{
    public sealed class SupervisorLogs : Supervisor, ISupervisorLogs
	{
        private readonly Lazy<ILogsRepository>? _lazyLogsRepository;

        #region Properties
        private ILogsRepository LogsRepository => _lazyLogsRepository!.Value;
        #endregion

        #region Constructor
        public SupervisorLogs(IDalSession session, IRepositoryFactory repositoryFactory) : base()
        {
            _lazyLogsRepository = repositoryFactory.CreateLogsRepository(session);
        }
        #endregion

        #region Methods
        public ResultCode AddLogs(Logs logs)
		{
			ResultCode result = ResultCode.CouldNotCreateItem;
            logs.Id = string.IsNullOrEmpty(logs.Id) ? Guid.NewGuid().ToString() : logs.Id;
            LogsEntity entity = LogsMapper.Map(logs);
            result = ((this.LogsRepository != null) && (this.LogsRepository.AddLogs(entity) == true)) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
			return result;
		}
		public async Task<(ResultCode, IEnumerable<Logs>?)> GetLogsAsync()
		{
			ResultCode result = ResultCode.ItemNotFound;
            IEnumerable<Logs>? logs = null;
            if (this.LogsRepository != null)
            {
                IEnumerable<LogsEntity>? entities = await this.LogsRepository.GetAllLogsAsync();
                logs = (entities != null) ? entities.Select((item) => LogsMapper.Map(item)) : null;
                result = ((logs != null) && (logs.Count() > 0)) ? ResultCode.Ok : ResultCode.ItemNotFound;
            }
			return (result, logs);
		}
        public ResultCode LogsExist(string id)
        {
            ResultCode result = ResultCode.ItemNotFound;
            if (this.LogsRepository != null)
            {
                result = (this.LogsRepository.LogsExists(id) == true) ? ResultCode.Ok : ResultCode.ItemNotFound;
            }
            return (result);
        }
        public async Task<(ResultCode, IEnumerable<Logs>?)> GetLogsInf24HAsync()
        {
            ResultCode result = ResultCode.ItemNotFound;
            IEnumerable<Logs>? logs = null;
            if (this.LogsRepository != null)
            {
                IEnumerable<LogsEntity>? entities = (await this.LogsRepository.GetAllLogsAsync())?.Where(arg => (Clock.Now <= arg.CreationDateTime.AddHours(24f)));
                logs = (entities != null) ? entities.Select((item) => LogsMapper.Map(item)) : null;
                result = ((logs != null) && (logs.Count() > 0)) ? ResultCode.Ok : ResultCode.ItemNotFound;
            }
            return (result, logs);
        }
        #endregion
    }
}

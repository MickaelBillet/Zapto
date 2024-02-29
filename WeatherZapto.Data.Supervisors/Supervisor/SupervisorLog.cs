using Framework.Core.Base;
using Framework.Core.Domain;
using Framework.Data.Abstractions;
using WeatherZapto.Data.Entities;
using WeatherZapto.Data.Mappers;
using WeatherZapto.Data.Services.Repositories;

namespace WeatherZapto.Data.Supervisors
{
    public partial class SupervisorLogs : ISupervisorLogs
	{
        private readonly Lazy<IRepository<LogsEntity>> _lazyLogsRepository;

        #region Properties
        private IRepository<LogsEntity> LogsRepository => _lazyLogsRepository?.Value;
        #endregion

        #region Constructor
        public SupervisorLogs(IDalSession session, IRepositoryFactory repositoryFactory)
        {
            _lazyLogsRepository = repositoryFactory?.CreateRepository<LogsEntity>(session);
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<Logs>> GetLogsInf24H()
		{
            IEnumerable<LogsEntity> entities = (await this.LogsRepository.GetCollectionAsync(arg => (Clock.Now.ToUniversalTime() <= arg.CreationDateTime.AddHours(24f).ToUniversalTime())));
            return (entities != null) ? entities.Select(item => LogsMapper.Map(item)) : Enumerable.Empty<Logs>();
		}

		public async Task<IEnumerable<Logs>> GetLogsCollection()
		{
            IEnumerable<LogsEntity> entities = await this.LogsRepository.GetCollectionAsync();
            return (entities != null) ? entities.Select(item => LogsMapper.Map(item)) : Enumerable.Empty<Logs>();
        }

        public async Task<ResultCode> AddLog(Logs log)
        {
            log.Id = string.IsNullOrEmpty(log.Id) ? Guid.NewGuid().ToString() : log.Id;
            log.Date = Clock.Now;
            int res = await this.LogsRepository.InsertAsync(LogsMapper.Map(log));
            ResultCode result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
            return result;
        }

        public async Task<ResultCode> LogExists(string id)
        {
            return (await this.LogsRepository.GetAsync(id) != null) ? ResultCode.Ok : ResultCode.ItemNotFound;
        }

        public async Task<Logs> GetLogs(string id)
        {
            return LogsMapper.Map(await this.LogsRepository.GetAsync(id));
        }

        public async Task<ResultCode> DeleteLogs(Logs log)
        {
            return (await this.LogsRepository.DeleteAsync(LogsMapper.Map(log)) > 0) ? ResultCode.Ok : ResultCode.CouldNotDeleteItem;
        }
        #endregion
    }
}

using Connect.Data.Entities;
using Connect.Data.Mappers;
using Connect.Data.Services.Repositories;
using Framework.Core.Base;
using Framework.Core.Domain;
using Framework.Data.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorLog : ISupervisorLog
	{
        private readonly Lazy<IRepository<LogsEntity>> _lazyLogRepository;

        #region Properties
        private IRepository<LogsEntity> LogsRepository => _lazyLogRepository.Value;
        #endregion

        #region Constructor
        public SupervisorLog(IDataContextFactory dataContextFactory, IRepositoryFactory repositoryFactory, IConfiguration configuration)
        {
            ConnectionType type = new ConnectionType()
            {
                ConnectionString = configuration["ConnectionStrings:DefaultConnection"],
                ServerType = ConnectionType.GetServerType(configuration["ConnectionStrings:ServerType"]),
            };

            IDataContext? context = dataContextFactory.CreateDbContext(type.ConnectionString, type.ServerType)?.context;
            if (context != null)
            {
                _lazyLogRepository = repositoryFactory.CreateRepository<LogsEntity>(context);
            }
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<Logs>> GetLogsInf24H()
		{
            IEnumerable<LogsEntity> entities = (await this.LogsRepository.GetCollectionAsync(arg => (Clock.Now <= arg.CreationDateTime.AddHours(24f))));
            return entities.Select(item => LogsMapper.Map(item));
		}

		public async Task<IEnumerable<Logs>> GetLogsCollection()
		{
            IEnumerable<LogsEntity> entities = await this.LogsRepository.GetCollectionAsync();
            return entities.Select(item => LogsMapper.Map(item));
        }

        public async Task<ResultCode> AddLog(Logs log)
        {
            log.Id = string.IsNullOrEmpty(log.Id) ? Guid.NewGuid().ToString() : log.Id;
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

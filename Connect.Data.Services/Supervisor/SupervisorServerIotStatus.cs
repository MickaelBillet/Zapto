using Connect.Data.Mappers;
using Connect.Data.Services.Repositories;
using Connect.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorServerIotStatus : ISupervisorServerIotStatus
	{
        private readonly Lazy<IServerIotStatusRepository> _lazyServerIotStatusRepository;

        #region Properties
        private IServerIotStatusRepository ServerIotStatusRepository => _lazyServerIotStatusRepository.Value;
        #endregion

        #region Constructor
        public SupervisorServerIotStatus(IDataContextFactory dataContextFactory, IRepositoryFactory repositoryFactory, IConfiguration configuration)
        {
            ConnectionType type = new ConnectionType()
            {
                ConnectionString = configuration["ConnectionStrings:DefaultConnection"],
                ServerType = ConnectionType.GetServerType(configuration["ConnectionStrings:ServerType"]),
            };

            IDataContext? context = dataContextFactory.CreateDbContext(type.ConnectionString, type.ServerType)?.context;
            if (context != null)
            {
                _lazyServerIotStatusRepository = repositoryFactory.CreateServerIotStatusRepository(context);
            }
        }
        #endregion

        #region Methods
        public async Task<ResultCode> AddServerIotStatus(ServerIotStatus serverIotStatus)
        {
            serverIotStatus.Id = string.IsNullOrEmpty(serverIotStatus.Id) ? Guid.NewGuid().ToString() : serverIotStatus.Id;
            int res = await this.ServerIotStatusRepository.InsertAsync(ServerIotStatusMapper.Map(serverIotStatus));
            ResultCode result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
            return result;
        }

        public async Task<int> CreateTable()
        {
            return await this.ServerIotStatusRepository.CreateTable();
        }
        #endregion
    }
}

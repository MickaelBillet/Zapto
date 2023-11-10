using Connect.Data.Mappers;
using Connect.Data.Services.Repositories;
using Connect.Data.Session;
using Connect.Model;
using Framework.Core.Base;
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
        public SupervisorServerIotStatus(IDalSession session, IRepositoryFactory repositoryFactory)
        {
            _lazyServerIotStatusRepository = repositoryFactory.CreateServerIotStatusRepository(session);
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

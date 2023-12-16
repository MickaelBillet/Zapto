using Connect.Data.Mappers;
using Connect.Data.Services.Repositories;
using Connect.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;

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

        public async Task<ResultCode> CreateTable()
        {
            return (await this.ServerIotStatusRepository.CreateTable() > 0) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
        }
        #endregion
    }
}

using Connect.Data.Entities;
using Connect.Data.Services.Repositories;
using Connect.Data.Session;

namespace Connect.Data.Repositories
{
    public abstract class ServerIotStatusRepository : Repository<ServerIotStatusEntity>, IServerIotStatusRepository
    {
        #region Constructor
        public ServerIotStatusRepository(IDalSession session) : base(session) { }
        #endregion

        #region Methods
        public abstract Task<int> CreateTable();
        #endregion
    }
}

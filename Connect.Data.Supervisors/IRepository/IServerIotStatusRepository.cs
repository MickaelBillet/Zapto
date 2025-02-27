using Connect.Data.Entities;
using Framework.Data.Abstractions;

namespace Connect.Data.Services.Repositories
{
    public interface IServerIotStatusRepository : IRepository<ServerIotStatusEntity>
    {
        Task<int> CreateTableServerIotStatus();
    }
}

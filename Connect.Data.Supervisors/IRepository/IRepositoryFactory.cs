using Framework.Core.Data;
using Framework.Data.Abstractions;

namespace Connect.Data.Services.Repositories
{
    public interface IRepositoryFactory
	{
        Lazy<IRepository<T>> CreateRepository<T>(IDalSession session) where T : ItemEntity;
        Lazy<IRoomRepository> CreateRoomRepository(IDalSession session);
        Lazy<IServerIotStatusRepository> CreateServerIotStatusRepository(IDalSession session);
    }
}

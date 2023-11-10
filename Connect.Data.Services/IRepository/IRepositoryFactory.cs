using Connect.Data.Session;
using Framework.Core.Data;
using Framework.Data.Abstractions;
using System;

namespace Connect.Data.Services.Repositories
{
    public interface IRepositoryFactory
	{
        Lazy<IRepository<T>>? CreateRepository<T>(IDalSession session) where T : ItemEntity;
        Lazy<IRoomRepository>? CreateRoomRepository(IDalSession session);
        Lazy<IServerIotStatusRepository>? CreateServerIotStatusRepository(IDalSession session);
    }
}

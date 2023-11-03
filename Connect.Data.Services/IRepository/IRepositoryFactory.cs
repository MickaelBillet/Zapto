using Framework.Core.Data;
using Framework.Data.Abstractions;
using System;

namespace Connect.Data.Services.Repositories
{
    public interface IRepositoryFactory
	{
        Lazy<IRepository<T>>? CreateRepository<T>(IDataContext context) where T : ItemEntity;
        Lazy<IRoomRepository>? CreateRoomRepository(IDataContext context);
    }
}

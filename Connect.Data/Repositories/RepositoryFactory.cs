using Connect.Data.Services.Repositories;
using Framework.Core.Data;
using Framework.Data.Abstractions;

namespace Connect.Data.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
		public Lazy<IRepository<T>>? CreateRepository<T>(IDataContext context) where T : ItemEntity
        {
			return (context != null) ? new Lazy<IRepository<T>>(() => new Repository<T>(context)) : null;
		}
        public Lazy<IRoomRepository>? CreateRoomRepository(IDataContext context)
        {
            return (context != null) ? new Lazy<IRoomRepository>(() => new RoomRepository(context)) : null;
        }
    }
}
 
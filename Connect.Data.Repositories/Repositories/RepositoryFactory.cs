using Connect.Data.Services.Repositories;
using Framework.Core.Base;
using Framework.Core.Data;
using Framework.Data.Abstractions;

namespace Connect.Data.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
		public Lazy<IRepository<T>>? CreateRepository<T>(IDalSession session) where T : ItemEntity
        {
			return (session?.DataContextFactory != null) ? new Lazy<IRepository<T>>(() => new Repository<T>(session.DataContextFactory)) : null;
		}
        public Lazy<IRoomRepository>? CreateRoomRepository(IDalSession session)
        {
            return (session?.DataContextFactory != null) ? new Lazy<IRoomRepository>(() => new RoomRepository(session.DataContextFactory)) : null;
        }
        public Lazy<IServerIotStatusRepository>? CreateServerIotStatusRepository(IDalSession session) 
        {
            Lazy<IServerIotStatusRepository>? serverIotStatusRepository = null;
            if (session?.DataContextFactory != null)
            {
                if (session.ConnectionType?.ServerType == ServerType.SqlLite)
                {
                    serverIotStatusRepository = new Lazy<IServerIotStatusRepository>(() => new ServerIotStatusRepositorySqlite(session.DataContextFactory));
                }
            }
            return serverIotStatusRepository;
        }
        public Lazy<IPlugRepository>? CreatePlugRepository(IDalSession session)
        {
            Lazy<IPlugRepository>? plugRepository = null;
            if (session?.DataContextFactory != null)
            {
                if (session.ConnectionType?.ServerType == ServerType.SqlLite)
                {
                    plugRepository = new Lazy<IPlugRepository>(() => new PlugRepositorySqlite(session.DataContextFactory));
                }
            }
            return plugRepository;
        }
    }
}
 
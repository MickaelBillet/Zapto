using Framework.Core.Data;
using Framework.Data.Abstractions;
using WeatherZapto.Data.Repositories;
using WeatherZapto.Data.Services.Repositories;

namespace Connect.Data.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public Lazy<IRepository<T>>? CreateRepository<T>(IDalSession session) where T : ItemEntity
        {
            return (session != null) ? new Lazy<IRepository<T>>(() => new Repository<T>(session)) : null;
        }
        public Lazy<ICallRepository>? CreateCallRepository(IDalSession session)
        {
            return (session != null) ? new Lazy<ICallRepository>(() => new CallRepository(session)) : null;
        }
    }
}
 
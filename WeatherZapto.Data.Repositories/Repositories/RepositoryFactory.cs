using Framework.Core.Data;
using Framework.Data.Abstractions;
using Framework.Data.Repository;
using WeatherZapto.Data.Services.Repositories;

namespace WeatherZapto.Data.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public Lazy<IRepository<T>>? CreateRepository<T>(IDalSession session) where T : ItemEntity
        {
            return (session.DataContextFactory != null) ? new Lazy<IRepository<T>>(() => new Repository<T>(session.DataContextFactory)) : null;
        }
        public Lazy<ICallRepository>? CreateCallRepository(IDalSession session)
        {
            return (session.DataContextFactory != null) ? new Lazy<ICallRepository>(() => new CallRepository(session.DataContextFactory)) : null;
        }
    }
}
 
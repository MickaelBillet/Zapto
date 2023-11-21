using Framework.Core.Data;
using Framework.Data.Abstractions;
using WeatherZapto.Data.Repositories;
using WeatherZapto.Data.Services.Repositories;

namespace Connect.Data.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public Lazy<IRepository<T>>? CreateRepository<T>(IDataContext context) where T : ItemEntity
        {
            return (context != null) ? new Lazy<IRepository<T>>(() => new Repository<T>(context)) : null;
        }
    }
}
 
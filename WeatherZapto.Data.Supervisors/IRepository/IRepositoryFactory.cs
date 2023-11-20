using Framework.Core.Data;
using Framework.Data.Abstractions;

namespace WeatherZapto.Data.Services.Repositories
{
    public interface IRepositoryFactory
	{
        Lazy<IRepository<T>> CreateRepository<T>(IDataContext context) where T : ItemEntity;
    }
}

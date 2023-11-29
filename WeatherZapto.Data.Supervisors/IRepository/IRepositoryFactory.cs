using Framework.Core.Data;
using Framework.Data.Abstractions;

namespace WeatherZapto.Data.Services.Repositories
{
    public interface IRepositoryFactory
	{
        Lazy<IRepository<T>> CreateRepository<T>(IDalSession session) where T : ItemEntity;
        Lazy<ICallRepository> CreateCallRepository(IDalSession session);
    }
}

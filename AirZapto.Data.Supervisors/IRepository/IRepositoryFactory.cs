using Framework.Data.Abstractions;

namespace AirZapto.Data.Services.Repositories
{
    public interface IRepositoryFactory
	{
        public Lazy<IRepository>? CreateRepository(IDataContext context);
    }
}

using Framework.Data.Abstractions;

namespace AirZapto.Data.Services.Repositories
{
    public interface IRepositoryFactory
    {
        Lazy<ILogsRepository>? CreateLogsRepository(IDalSession session);
        Lazy<ISensorDataRepository>? CreateSensorDataRepository(IDalSession session);
        Lazy<ISensorRepository>? CreateSensorRepository(IDalSession session);
        Lazy<IVersionRepository>? CreateVersionRepository(IDalSession session);
    }
}
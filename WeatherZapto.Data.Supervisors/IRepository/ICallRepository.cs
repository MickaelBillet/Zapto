using Framework.Data.Abstractions;
using WeatherZapto.Data.Entities;

namespace WeatherZapto.Data.Services.Repositories
{
    public interface ICallRepository : IRepository<CallEntity>
    {
        Task<long?> GetLast30DaysCallsCount();
    }
}

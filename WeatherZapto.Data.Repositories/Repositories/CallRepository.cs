using Framework.Core.Base;
using Framework.Data.Abstractions;
using Framework.Data.Repository;
using Microsoft.EntityFrameworkCore;
using WeatherZapto.Data.Entities;
using WeatherZapto.Data.Services.Repositories;

namespace WeatherZapto.Data.Repositories
{
    public class CallRepository : Repository<CallEntity>, ICallRepository
    {
        #region Constructor
        public CallRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory) { }
        #endregion

        #region Methods
        public async Task<long?> GetLast30DaysCallsCount()
        {
            long? count = 0;
            await this.DataContextFactory.UseContext(async (context) =>
            {
                if (context != null)
                {
                    DateTime date = Clock.Now - new TimeSpan(30, 0, 0, 0);
                    FormattableString sql = $"SELECT * FROM \"Call\" Where (\"CreationDateTime\" >= {date.ToUniversalTime()})";
                    await context.Set<CallEntity>().FromSql<CallEntity>(sql).ForEachAsync(item =>
                    {
                        count = count + item.Count;
                    });
                }
            });
            return count;
        }
        #endregion
    }
}

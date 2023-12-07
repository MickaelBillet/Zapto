using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using WeatherZapto.Data.Entities;
using WeatherZapto.Data.Services.Repositories;

namespace WeatherZapto.Data.Repositories
{
    public class CallRepository : Repository<CallEntity>, ICallRepository
    {
        #region Constructor
        public CallRepository(IDalSession session) : base(session) { }
        #endregion

        #region Methods
        public async Task<long?> GetLast30DaysCallsCount()
        {
            long? count = 0;
            if (this.DataContext != null)
            {
                DateTime date = Clock.Now - new TimeSpan(30,0,0,0);
                FormattableString sql = $"SELECT * FROM \"Call\" Where (\"CreationDateTime\" >= {date.ToUniversalTime()})"; 

                if (this.DataContext != null)
                {
                    await this.DataContext.CallEntities.FromSql<CallEntity>(sql).ForEachAsync(item => 
                    {
                        count = count + item.Count;
                    });
                }
            }
            return count;
        }
        #endregion
    }
}

using Framework.Core.Base;
using Framework.Data.Abstractions;
using WeatherZapto.Data.Entities;
using WeatherZapto.Data.Services.Repositories;

namespace WeatherZapto.Data.Supervisors
{
    public partial class SupervisorCall : ISupervisorCall
	{
        private readonly Lazy<ICallRepository> _lazyCallRepository;

        #region Properties
        private ICallRepository CallRepository => _lazyCallRepository?.Value;
        #endregion

        #region Constructor
        public SupervisorCall(IDalSession session, IRepositoryFactory repositoryFactory)
        {
            _lazyCallRepository = repositoryFactory?.CreateCallRepository(session);
        }
        #endregion

        #region Methods
        public async Task<ResultCode> AddCallOpenWeather()
        {
            int res = 0;

            //Search CallEntity between the beginning of the day and now
            CallEntity entity = await this.CallRepository.GetAsync((item) => item.CreationDateTime >= new DateTime(Clock.Now.Year, Clock.Now.Month, Clock.Now.Day).ToUniversalTime()
                                                                              && item.CreationDateTime <= DateTime.UtcNow);
            if (entity != null) 
            {
                entity.Count++;
                res = await this.CallRepository.UpdateAsync(entity);
            }
            else
            {
                res = await this.CallRepository.InsertAsync(new CallEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreationDateTime = Clock.Now.ToUniversalTime(),
                    Count = 1,
                });                
            }

            ResultCode result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
            return result;
        }

        public async Task<long?> GetDayCallsCount(DateTime day)
        {
            DateTime universalDateTime = day.ToUniversalTime();
            IEnumerable<CallEntity> entities = await this.CallRepository.GetCollectionAsync((item) => item.CreationDateTime >= new DateTime(day.Year, day.Month, day.Day).ToUniversalTime()
                                                                                                        && item.CreationDateTime <= new DateTime(day.Year, day.Month, day.Day + 1).ToUniversalTime());
            long? count = 0;
            entities.ToList().ForEach(entity =>
            {
                count = count + entity.Count;
            });
            return count;
        }

        public async Task<long?> GetLast30DaysCallsCount()
        {
            long? count = await this.CallRepository.GetLast30DaysCallsCount();
            return count;
        }
        #endregion
    }
}

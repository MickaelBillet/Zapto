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
        private static AutoResetEvent AutoReset = new AutoResetEvent(true);
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

            AutoReset.WaitOne();

            CallEntity entity = await this.CallRepository.GetAsync((item) => item.CreationDateTime.ToUniversalTime().Date.Year.Equals(Clock.Now.Year)
                                                                            && item.CreationDateTime.ToUniversalTime().Date.Month.Equals(Clock.Now.Month)
                                                                            && item.CreationDateTime.ToUniversalTime().Date.Day.Equals(Clock.Now.Day));
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

            AutoReset.Set();

            ResultCode result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
            return result;
        }

        public async Task<long?> GetDayCallsCount(DateTime day)
        {
            IEnumerable<CallEntity> entities = await this.CallRepository.GetCollectionAsync((item) => item.CreationDateTime.ToUniversalTime().Date.Year.Equals(day.Year)
                                                                                                        && item.CreationDateTime.ToUniversalTime().Date.Month.Equals(day.Month)
                                                                                                        && item.CreationDateTime.ToUniversalTime().Date.Day.Equals(day.Day));

            long? count = entities.Count();

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

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
            
            CallEntity entity = await this.CallRepository.GetAsync(item => item.CreationDateTime.ToUniversalTime().CompareDay(Clock.Now.ToUniversalTime()) == 0);
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

        public async Task<long?> GetDayCallsCount(DateTime date)
        {
            CallEntity entity = (await this.CallRepository.GetAsync((item) => item.CreationDateTime.ToUniversalTime().CompareDay(Clock.Now.ToUniversalTime()) == 0));
            return (entity != null) ? entity.Count : 0;
        }

        public async Task<long?> GetLast30DaysCallsCount()
        {
           long? count = await this.CallRepository.GetLast30DaysCallsCount();
            return count;
        }
        #endregion
    }
}

using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.Extensions.Configuration;
using WeatherZapto.Data.Entities;
using WeatherZapto.Data.Mappers;
using WeatherZapto.Data.Services.Repositories;
using WeatherZapto.Model;

namespace WeatherZapto.Data.Supervisors
{
    public partial class SupervisorCall : ISupervisorCall
	{
        private readonly Lazy<IRepository<CallEntity>> _lazyCallRepository;

        #region Properties
        private IRepository<CallEntity> CallRepository => _lazyCallRepository?.Value;
        #endregion

        #region Constructor
        public SupervisorCall(IDataContextFactory dataContextFactory, IRepositoryFactory repositoryFactory, IConfiguration configuration)
        {
            ConnectionType type = new ConnectionType()
            {
                ConnectionString = configuration["ConnectionStrings:DefaultConnection"],
                ServerType = ConnectionType.GetServerType(configuration["ConnectionStrings:ServerType"]),
            };

            IDataContext context = dataContextFactory.CreateDbContext(type.ConnectionString, type.ServerType)?.context;
            if (context != null)
            {
                if (repositoryFactory != null)
                {
                    _lazyCallRepository = repositoryFactory?.CreateRepository<CallEntity>(context);
                }
            }
        }
        #endregion

        #region Methods
        public async Task<ResultCode> AddCallOW()
        {
            int res = 0;
            CallEntity entity = await this.CallRepository.GetAsync(item => item.CreationDateTime.Date == Clock.Now.Date);
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
                    CreationDateTime = Clock.Now,
                    Count = 1,
                });                
            }

            ResultCode result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
            return result;
        }
        #endregion
    }
}

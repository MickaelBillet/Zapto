using Framework.Core.Base;
using Framework.Data.Abstractions;
using Microsoft.Extensions.Configuration;
using WeatherZapto.Data.Entities;
using WeatherZapto.Data.Mappers;
using WeatherZapto.Data.Services.Repositories;
using WeatherZapto.Model;

namespace WeatherZapto.Data.Supervisors
{
    public class SupervisorWeather : ISupervisorWeather
    {
        private readonly Lazy<IRepository<WeatherEntity>>? _lazyWeatherRepository;

        #region Properties
        private IRepository<WeatherEntity>? WeatherRepository => _lazyWeatherRepository?.Value;
        #endregion

        #region Constructor
        public SupervisorWeather(IDataContextFactory dataContextFactory, IRepositoryFactory? repositoryFactory, IConfiguration configuration)
        {
            ConnectionType type = new ConnectionType()
            {
                ConnectionString = configuration["ConnectionStrings:DefaultConnection"],
                ServerType = ConnectionType.GetServerType(configuration["ConnectionStrings:ServerType"]),
            };

            IDataContext? context = dataContextFactory.CreateDbContext(type.ConnectionString, type.ServerType)?.context;
            if (context != null)
            {
                if (repositoryFactory != null)
                {
                    _lazyWeatherRepository = repositoryFactory?.CreateRepository<WeatherEntity>(context);
                }
            }
        }
        #endregion

        #region Methods
        public async Task<ResultCode> WeatherExists(string id)
        {
            return (await this.WeatherRepository.GetAsync(id) != null) ? ResultCode.Ok : ResultCode.ItemNotFound;
        }

        public async Task<ResultCode> AddWeatherAsync(ZaptoWeather weather)
        {
            ResultCode result = await this.WeatherExists(weather?.Id);
            if (result == ResultCode.ItemNotFound)
            {
                weather.Id = string.IsNullOrEmpty(weather.Id) ? Guid.NewGuid().ToString() : weather.Id;
                weather.Date = Clock.Now.ToUniversalTime();
                int res = await this.WeatherRepository.InsertAsync(OpenWeatherMapper.Map(weather));
                result = (res > 0) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
            }
            else
            {
                result = ResultCode.ItemAlreadyExist;
            }

            return result;
        }

        public async Task<ResultCode> DeleteWeatherAsync(ZaptoWeather weather)
        {
            return (await this.WeatherRepository.DeleteAsync(OpenWeatherMapper.Map(weather)) > 0) ? ResultCode.Ok : ResultCode.CouldNotDeleteItem;
        }
        #endregion
    }
}

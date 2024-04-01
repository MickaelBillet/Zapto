using WeatherZapto.Application.Infrastructure;

namespace WeatherZapto.Infrastructure.WebServices
{
    public class TemperatureService : WeatherZaptoWebService, ITemperatureService
    {
        #region Constructor
        public TemperatureService(IServiceProvider serviceProvider, string httpClientName) : base(serviceProvider, httpClientName)
        {

        }
        #endregion

        #region Method
        public async Task<IEnumerable<double?>> GetLocationTemperatures(DateTime dateTime, string location, CancellationToken cancellationToken = default)
        {
            return await WebService.GetCollectionAsync<double?>(string.Format(WeatherZaptoConstants.UrlTemperaturesDay, location, dateTime.ToString("yyyy-MM-ddTHH:mm:ss")), this.SerializerOptions, cancellationToken); ;
        }
        #endregion
    }
}

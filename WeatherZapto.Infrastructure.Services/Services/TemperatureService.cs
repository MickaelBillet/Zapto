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
        public async Task<IEnumerable<double?>> GetHomeTemperatures(DateTime? day, string location, CancellationToken cancellationToken = default)
        {
            return await WebService.GetCollectionAsync<double?>(string.Format(WeatherZaptoConstants.UrlTemperaturesDay, day!.Value.ToString("dd-MM-yyyy"), location), SerializerOptions, cancellationToken); ;
        }
        #endregion
    }
}

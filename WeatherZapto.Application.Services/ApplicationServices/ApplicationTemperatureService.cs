using Microsoft.Extensions.DependencyInjection;
using WeatherZapto.Application.Infrastructure;

namespace WeatherZapto.Application.Services
{
    internal class ApplicationTemperatureService : IApplicationTemperatureService
    {
        #region Services
        private ITemperatureService TemperatureService { get; }
        #endregion

        #region Constructor
        public ApplicationTemperatureService(IServiceProvider serviceProvider)
        {
            this.TemperatureService = serviceProvider.GetRequiredService<ITemperatureService>();
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<double?>> GetTemperatureOfDay(string location, DateTime? day, CancellationToken token = default)
        {
            IEnumerable<double?> temperatures = (this.TemperatureService != null) ? await this.TemperatureService.GetHomeTemperatures(day, location, token) : null;
            return (temperatures ?? Enumerable.Empty<double?>());
        }
        #endregion
    }
}

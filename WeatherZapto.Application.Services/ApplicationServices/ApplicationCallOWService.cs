using Microsoft.Extensions.DependencyInjection;
using WeatherZapto.Data;

namespace WeatherZapto.Application.Services
{ 
    internal class ApplicationCallOWService : IApplicationCallOWService
    {
        #region Services
        private ISupervisorCall SupervisorCall { get; }
        #endregion

        #region Constructor
        public ApplicationCallOWService(IServiceProvider serviceProvider) 
        {
            this.SupervisorCall = serviceProvider.GetRequiredService<ISupervisorCall>();
        }
        #endregion

        #region Methods
        public async Task <int>GetCurrentDayCallsCount()
        {
            int res = 0;

            return await Task.FromResult<int>(res);
        }

        public async Task<long> GetLast30DaysCallsCount()
        {
            long res = 0;

            return await Task.FromResult<long>(res);
        }
        #endregion
    }
}

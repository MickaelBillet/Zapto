using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Connect.Infrastructure.WebServices
{
    public class SensorService : ConnectWebService, ISensorService
    {
        #region Constructor

        public SensorService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
        {
        }

        #endregion

        #region Method

        /// <summary>
        /// Leak
        /// </summary>
        public async Task<bool?> Leak(string? sensorId, string status)
        {
            return await WebService.PutAsync<string>(ConnectConstants.RestUrlSensorNoLeak, status, sensorId); ;
        }

        #endregion
    }
}

using AirZapto.Application.Infrastructure;
using AirZapto.Model;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AirZapto.Infrastructure.WebServices
{
    internal sealed class SensorDataService : AirZaptoWebService, ISensorDataService
	{
        #region Property
        #endregion

        #region Constructor
        public SensorDataService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
        {

        }
        #endregion

        #region Method

        public async Task<IEnumerable<AirZaptoData>?> GetSensorData(string? sensorId, int duration)
        {
            IEnumerable<AirZaptoData>? data = null;

            try
            {
                //Call the webservice
                data = await this.WebService.GetCollectionAsync<AirZaptoData>(string.Format(AirZaptoConstants.RestUrlSensorData, sensorId, duration), 
                                                                                this.SerializerOptions, 
                                                                                new CancellationToken());
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }

            return data;
        }

		#endregion
	}
}

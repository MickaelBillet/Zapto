using AirZapto.Application.Infrastructure;
using AirZapto.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AirZapto.Infrastructure.WebServices
{
    internal sealed class SensorService : AirZaptoWebService, ISensorService
    {
        #region Property

        #endregion

        #region Constructor

        public SensorService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
        {
        }

        #endregion

        #region Method

        public async Task<IEnumerable<Sensor>?> GetSensors()
        {
            return await this.WebService.GetCollectionAsync<Sensor>(AirZaptoConstants.RestUrlSensors, this.SerializerOptions, new CancellationToken());
        }

        public async Task<Sensor?> GetSensor(string idSocket)
        {
            return await this.WebService.GetAsync<Sensor>(AirZaptoConstants.RestUrlSensor,
                                                                    idSocket,
                                                                    this.SerializerOptions,
                                                                    new CancellationToken());
        }

        public async Task<bool?> CalibrationSensor(Sensor sensor)
        {
            return await this.WebService.PutAsync<int[]>(AirZaptoConstants.RestUrlSensorCalibration,
                                                            Array.Empty<int>(),
                                                            sensor.IdSocket);
        }

        public async Task<bool?> RestartSensor(Sensor sensor)
        {
            return await this.WebService.PutAsync<int[]>(AirZaptoConstants.RestUrlSensorRestart,
                                                            Array.Empty<int>(),
                                                            sensor.IdSocket);
        }

        #endregion
    }
}

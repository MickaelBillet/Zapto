using AirZapto.Application.Infrastructure;
using AirZapto.Model;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			IEnumerable<Sensor>? sensors = null;

            try
            {
                //Call the webservice
                sensors = await this.WebService.GetCollectionAsync<Sensor>(AirZaptoConstants.RestUrlSensors, this.SerializerOptions, new CancellationToken());
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                Debug.Write(ex);

                throw;
            }

            return sensors;
        }

        public async Task<Sensor?> GetSensor(string sensorId)
        {
            Sensor? sensor = null;

            try
            {
                //Call the webservice
                sensor = await this.WebService.GetAsync<Sensor>(AirZaptoConstants.RestUrlSensor,
                                                                    sensorId,
                                                                    this.SerializerOptions, 
                                                                    new CancellationToken());
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return sensor;
        }

        public async Task<bool?> CalibrationSensor(Sensor sensor)
        {
            bool? res = false;

            try
            {
                res = await this.WebService.PutAsync<int[]>(AirZaptoConstants.RestUrlSensorCalibration, 
                                                            Array.Empty<int>(), 
                                                            sensor.Id);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return res;
        }

        public async Task<bool?> RestartSensor(Sensor sensor)
        {
            bool? res = false;

            try
            {
                res = await this.WebService.PutAsync<int[]>(AirZaptoConstants.RestUrlSensorRestart, 
                                                            Array.Empty<int>(),
                                                            sensor.Id);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return res;
        }

        #endregion
    }
}

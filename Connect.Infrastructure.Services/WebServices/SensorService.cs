﻿using Connect.Application.Infrastructure;
using Connect.Model;
using System;
using System.Threading.Tasks;

namespace Connect.Infrastructure.WebServices
{
    public class SensorService : ConnectWebService, ISensorService
    {
        #region Constructor

        public SensorService(IServiceProvider serviceProvider, string httpClientName) : base(serviceProvider, httpClientName)
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

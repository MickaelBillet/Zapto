﻿using AirZapto.Application.Infrastructure;
using AirZapto.Model.Healthcheck;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AirZapto.Infrastructure.WebServices
{
    internal sealed class HealthCheckAirZaptoService : AirZaptoWebService, IHealthCheckAirZaptoService
    {
        #region Constructor
        public HealthCheckAirZaptoService(IServiceProvider serviceProvider, IConfiguration configuration, string httpClientName) : base(serviceProvider, configuration, httpClientName)
        {
        }
        #endregion

        #region Method  

        public async Task<HealthCheckAirZapto?> GetHealthCheckAirZapto(CancellationToken token = default)
        {
            return await this.WebService.GetAsync<HealthCheckAirZapto>(AirZaptoConstants.RestUrlHealthCheck, null, null, token);
        }
        #endregion
    }
}

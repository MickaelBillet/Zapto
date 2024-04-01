using AirZapto.Data;
using AirZapto.Data.Services;
using Framework.Core.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirZapto.WebServices.Controllers
{
    [Authorize(Policy = "user")]
    [ApiController]
    [Route("[controller]")]
    public class SensorDataController : ControllerBase
    {
        #region Property

        private ISupervisorSensorData Supervisor { get; }

        #endregion

        #region Constructor

        public SensorDataController(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.Supervisor = serviceProvider.GetRequiredService<ISupervisorSensorData>();
        }

        #endregion

        #region Method        

        //GET sensordata
        [HttpGet("~/sensordata/{sensorId}/duration/{minutes}")]
        public async Task<IActionResult> Get(string sensorId, int minutes)
        {
            try
            {
                (ResultCode code, IEnumerable<Model.AirZaptoData>? data) = await this.Supervisor.GetSensorDataAsync(sensorId, minutes);

                if (code == ResultCode.Ok)
                {
                    return StatusCode(200, data);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CustomErrorResponse
                {
                    Message = ex.Message,
                    Description = string.Empty,
                    Code = 500,
                });
            }
        }

        #endregion
    }
}

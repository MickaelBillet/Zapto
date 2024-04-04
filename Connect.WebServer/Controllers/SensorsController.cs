using Connect.Data;
using Connect.Model;
using Framework.Core.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.WebApi.Controllers
{
    [Route("connect/[controller]")]
    [Authorize(Policy = "user")]
    public class SensorsController : Controller
    {
        #region Property
        ISupervisorSensor SupervisorSensor { get; }
        #endregion

        #region Constructor

        public SensorsController(IServiceProvider serviceProvider)
        {
            this.SupervisorSensor = serviceProvider.GetRequiredService<ISupervisorSensor>();
        }

        #endregion

        #region Method

        //ConnectConstants.RestUrlSensorNoLeak
        //PUT connect/sensors/5/noleak
        [HttpPut("~/connect/Sensors/{id}/noleak/")]
        public async Task<IActionResult> Put(string id, [FromBody] string status)
        {
            try
            {
                if ((status == null) || (id == null))
                {
                    return BadRequest(new CustomErrorResponse
                    {
                        Message = ResultCode.ArgumentRequired.ToString(),
                        Description = string.Empty,
                        Code = 400,
                    });
                }
                else if (!ModelState.IsValid)
                {
                    return BadRequest(new CustomErrorResponse
                    {
                        Message = ResultCode.CouldNotUpdateItem.ToString(),
                        Description = string.Empty,
                        Code = 400,
                    });
                }
                else
                {
                    Sensor sensor = await this.SupervisorSensor.GetSensor(id);
                    if (sensor != null)
                    {
                        sensor.LeakDetected = int.Parse(status);
                        ResultCode resultCode = ResultCode.CouldNotUpdateItem;
                        (resultCode, sensor) = await this.SupervisorSensor.UpdateSensor(sensor);
                        return StatusCode(200, resultCode);
                    }
                    else
                    {
                        return NotFound();
                    }
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

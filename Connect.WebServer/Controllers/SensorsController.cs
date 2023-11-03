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

        // GET connect/sensors
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<Sensor>? sensors;

            try
            {
                sensors = await this.SupervisorSensor.GetSensors();

                if (sensors != null)
                {
                    return StatusCode(200, sensors);
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

        // GET connect/sensors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Sensor? sensor;

            try
            {
                sensor = await this.SupervisorSensor.GetSensor(id);

                if (sensor != null)
                {
                    return StatusCode(200, sensor);
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

        // GET connect/rooms/5/sensors
        [HttpGet("~/connect/Rooms/{id}/Sensors")]
        public async Task<IActionResult> GetFromRoom(string roomId)
        {
            IEnumerable<Sensor>? sensors;

            try
            {
                sensors = await this.SupervisorSensor.GetSensors(roomId);

                if (sensors != null)
                {
                    return StatusCode(200, sensors);
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

        // POST connect/sensors
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Sensor sensor)
        {
            try
            {
                if (sensor == null || !ModelState.IsValid)
                {
                    return BadRequest(new CustomErrorResponse
                    {
                        Message = ResultCode.ArgumentRequired.ToString(),
                        Description = string.Empty,
                        Code = 400,
                    });
                }

                ResultCode resultCode = await this.SupervisorSensor.AddSensor(sensor);

                if (resultCode == ResultCode.Ok)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(405, new CustomErrorResponse
                    {
                        Message = resultCode.ToString(),
                        Description = string.Empty,
                        Code = 405,
                    });
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

        //DELETE connect/sensors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                Sensor sensor = await this.SupervisorSensor.GetSensor(id);

                if (sensor != null)
                {
                    return StatusCode(200, await this.SupervisorSensor.DeleteSensor(sensor));
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

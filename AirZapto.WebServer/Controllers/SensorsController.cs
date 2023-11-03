using AirZapto.Application;
using AirZapto.Data;
using AirZapto.Model;
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
    public class SensorsController : ControllerBase
    {
        #region Property
        private ISupervisorSensor Supervisor { get; }
        private IApplicationSensorServices ApplicationSensorServices { get; }
        #endregion

        #region Constructor
        public SensorsController(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.Supervisor = serviceProvider.GetRequiredService<ISupervisorSensor>();
            this.ApplicationSensorServices = serviceProvider.GetRequiredService<IApplicationSensorServices>();
        }
        #endregion

        #region Method
        //GET sensors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                (ResultCode code, Sensor? sensor) = await this.Supervisor.GetSensorAsync(id);

                if (code == ResultCode.Ok)
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

        //GET sensors
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            try
            {
                (ResultCode code, IEnumerable<Sensor>? sensors) = await this.Supervisor.GetSensorsAsync();

                if (code == ResultCode.Ok)
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

        //PUT sensors/5/calibration/
        [HttpPut("~/sensors/{id}/calibration/")]
        public async Task<IActionResult> Calibration(string id)
        {
            try
            {
                if (id == null)
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
                    (ResultCode code, Sensor? sensor) = await this.Supervisor.GetSensorAsync(id);

                    if ((code == ResultCode.Ok) && (sensor != null))
                    {
                        bool isConnected = await this.ApplicationSensorServices.SendCommandAsync(sensor, Command.Calibration);

                        if (isConnected == false)
						{
                            sensor.Mode = SensorMode.Initial;
                        }

                        code = await this.Supervisor.UpdateSensorAsync(sensor);

                        if (code == ResultCode.Ok)
                        {
                            return Ok();
                        }
                        else if (code == ResultCode.ItemNotFound)
                        {
                            return NotFound(new CustomErrorResponse
                            {
                                Message = code.ToString(),
                                Description = string.Empty,
                                Code = 404,
                            });
                        }
                        else
                        {
                            return StatusCode(405, new CustomErrorResponse
                            {
                                Message = code.ToString(),
                                Description = "Command Error",
                                Code = 405,
                            });
                        }
                    }
                    else
                    {
                        return NotFound(new CustomErrorResponse
                        {
                            Message = ResultCode.ItemNotFound.ToString(),
                            Description = string.Empty,
                            Code = 404,
                        });
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
        
        //PUT sensors/5/restart/
        [HttpPut("~/sensors/{id}/restart/")]
        public async Task<IActionResult> Restart(string id)
        {
            try
            {
                if (id == null)
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
                    (ResultCode code, Sensor? sensor) = await this.Supervisor.GetSensorAsync(id);

                    if ((code == ResultCode.Ok) && (sensor != null))
                    {
                        bool isConnected = await this.ApplicationSensorServices.SendCommandAsync(sensor, Command.Restart);

                        if (isConnected == false)
                        {
                            sensor.Mode = SensorMode.Initial;
                        }

                        code = await this.Supervisor.UpdateSensorAsync(sensor);

                        if (code == ResultCode.Ok)
                        {
                            return Ok();
                        }
                        else if (code == ResultCode.ItemNotFound)
                        {
                            return NotFound(new CustomErrorResponse
                            {
                                Message = code.ToString(),
                                Description = string.Empty,
                                Code = 404,
                            });
                        }
                        else
                        {
                            return StatusCode(405, new CustomErrorResponse
                            {
                                Message = code.ToString(),
                                Description = "Command Error",
                                Code = 405,
                            });
                        }
                    }
                    else
                    {
                        return NotFound(new CustomErrorResponse
                        {
                            Message = ResultCode.ItemNotFound.ToString(),
                            Description = string.Empty,
                            Code = 404,
                        });
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

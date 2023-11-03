using Connect.Application;
using Connect.Data;
using Connect.Model;
using Framework.Core.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Connect.WebApi.Controllers
{
    [Route("connect/[controller]")]
    [Authorize(Policy = "user")]
    public class PlugsController : Controller
    {
        #region Property

        private ISupervisorPlug SupervisorPlug { get; }
        private ISupervisorRoom SupervisorRoom { get; }
        private IApplicationPlugServices ApplicationPlugServices { get; }


        #endregion

        #region Constructor

        public PlugsController(IServiceProvider serviceProvider)
        {
            this.SupervisorPlug = serviceProvider.GetRequiredService<ISupervisorPlug>();
            this.ApplicationPlugServices = serviceProvider.GetRequiredService<IApplicationPlugServices>();
            this.SupervisorRoom = serviceProvider.GetRequiredService<ISupervisorRoom>();
        }

        #endregion

        #region HttpRequest

        // GET connect/plugs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Plug? plug;

            try
            {
                plug = await this.SupervisorPlug.GetPlug(id);

                if (plug != null)
                {
                    return StatusCode(200, plug);
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

        //PUT connect/plugs/5/mode/
        [HttpPut("~/connect/Plugs/{id}/mode/")]
        [HttpPut("~/connect/Plugs/{id}/onoff/")]
        public async Task<IActionResult> Put(string id, [FromBody]int[] command)
        {
            try
            {
                if ((command == null) || (id == null))
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
                    Plug plug = await this.SupervisorPlug.GetPlug(id);

                    if (plug != null)
                    {                        
                        plug.OnOff = command[0];
                        plug.Mode = command[1];

                        ResultCode resultCode = ResultCode.CouldNotUpdateItem;

                        Room room = await this.SupervisorRoom.GetRoomFromPlugId(plug.Id);
                        if (room != null)
                        {
                            //Get order before the update
                            int previousOrder = plug.Order;

                            //Update the order with all the data
                            plug.UpdateOrder(room.Humidity, room.Temperature);

                            //We send the command to the Arduino when the order changes
                            if (plug.Order != previousOrder)
                            {
                                //Send Command to Arduino
                                if (await this.ApplicationPlugServices.SendCommandAsync(plug) <= 0)
                                {
                                    //If we cannot send the command to the Arduino, we keep the previous value
                                    plug.Order = previousOrder;
                                }
                            }

                            Log.Information("UpdateOrderPlug " + "Id : " + plug.Id);
                            Log.Information("UpdateOrderPlug " + "OnOff : " + plug.OnOff);
                            Log.Information("UpdateOrderPlug " + "Status : " + plug.Status);
                            Log.Information("UpdateOrderPlug " + "Order : " + plug.Order);

                            //Update the working duration
                            plug.ComputeWorkingDuration();

                            //Send Status to the clients app (Mobile, Web...)
                            await this.ApplicationPlugServices.SendStatusToClientAsync(room.LocationId, plug);
                            resultCode = await this.SupervisorPlug.UpdatePlug(plug);
                        }
                        
                        if (resultCode == ResultCode.Ok)
                        {
                            return Ok();
                        }
                        else if (resultCode == ResultCode.ItemNotFound)
                        {
                            return NotFound(new CustomErrorResponse
                            {
                                Message = resultCode.ToString(),
                                Description = string.Empty,
                                Code = 404,
                            });
                        }
                        else
                        {
                            return StatusCode(405, new CustomErrorResponse
                            {
                                Message = resultCode.ToString(),
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

using Connect.Application;
using Connect.Data;
using Connect.Model;
using Connect.WebServer.Services;
using Framework.Core.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Connect.WebApi.Controllers
{
    [Route("connect/[controller]")]
    [Authorize(Policy = "user")]
    public class PlugsController : Controller
    {
        #region Services
        private ISupervisorPlug SupervisorPlug { get; }
        private ISendCommandService SendCommandService { get; }
        #endregion

        #region Constructor
        public PlugsController(IServiceProvider serviceProvider)
        {
            this.SupervisorPlug = serviceProvider.GetRequiredService<ISupervisorPlug>();
            this.SendCommandService = serviceProvider.GetRequiredService<ISendCommandService>();
        }
        #endregion

        #region HttpRequest
        //ConnectConstants.RestUrlPlugCommand
        //ConnectConstants.RestUrlPlugOrder
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
                    Plug? plug = await this.SupervisorPlug.GetPlug(id);
                    if (plug != null)
                    {                        
                        plug.OnOff = command[0];
                        plug.Mode = command[1];

                        ResultCode resultCode = await this.SendCommandService.Execute(plug);                        
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

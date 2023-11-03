using Connect.Data;
using Connect.Model;
using Framework.Core.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.WebApi.Controllers
{
    [Route("connect/[controller]")]
    public class ClientAppsController : Controller
    {
        #region Property
        ISupervisorClientApps Supervisor { get; }
        #endregion

        #region Constructor

        public ClientAppsController(IServiceProvider serviceProvider)
        {
            this.Supervisor = serviceProvider.GetRequiredService<ISupervisorClientApps>();
        }

        #endregion

        #region Method

        // GET connect/clientapps
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<ClientApp>? clientApps;

            try
            {
                clientApps = await this.Supervisor.GetClientApps();

                if (clientApps != null)
                {
                    return StatusCode(200, clientApps);
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

        // GET connect/clientapps/token/5
        [HttpGet("~/connect/Clientapps/token/{token}")]
        public async Task<IActionResult> Get(string token)
        {
            ClientApp? clientApp;

            try
            {
                clientApp = await this.Supervisor.GetClientAppFromToken(token);
                if (clientApp != null)
                {
                    return StatusCode(200, clientApp);
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

        // DELETE connect/clientapps/token/5
        [HttpDelete("~/connect/Clientapps/token/{token}")]
        public async Task<IActionResult> Delete(string token)
        {
            ClientApp? clientApp;

            try
            {
                clientApp = await this.Supervisor.GetClientAppFromToken(token);
                if (clientApp != null)
                {
                    ResultCode resultCode = await this.Supervisor.DeleteClientApp(clientApp);
                    if (resultCode == ResultCode.Ok)
                    {
                        return StatusCode(200, clientApp);
                    }
                    else
                    {
                        return StatusCode(409, new CustomErrorResponse
                        {
                            Message = resultCode.ToString(),
                            Description = string.Empty,
                            Code = 409,
                        });
                    }
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

        // POST connect/clientapps
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClientApp clientApp)
        {
            try
            {
                if (clientApp == null || !ModelState.IsValid)
                {
                    return BadRequest(new CustomErrorResponse
                    {
                        Message = ResultCode.ArgumentRequired.ToString(),
                        Description = string.Empty,
                        Code = 400,
                    });
                }

                ResultCode resultCode = await this.Supervisor.AddClientApp(clientApp);
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

        #endregion
    }
}

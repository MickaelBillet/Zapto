using Connect.Data;
using Framework.Core.Base;
using Framework.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.WebApi.Controllers
{
    [Route("connect/[controller]")]
    [Authorize(Policy = "administrator")]
    public class LogsController : Controller
	{
		#region Property
		ISupervisorLog SupervisorLog { get; }
		#endregion

		#region Constructor
		public LogsController(IServiceProvider serviceProvider)
		{
			this.SupervisorLog = serviceProvider.GetRequiredService<ISupervisorLog>();
		}
        #endregion

        #region Method
        // GET connect/Logs
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<Logs>? logs;

            try
            {
                logs = await this.SupervisorLog.GetLogsCollection();
                if (logs != null)
                {
                    return StatusCode(200, logs);
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
                    Message     = ex.Message,
                    Description = string.Empty,
                    Code        = 500,
                });
            }
        }

        // POST connect/Logs
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Logs log)
        {
            try
            {
                if (log == null || !ModelState.IsValid)
                {
                    return BadRequest(new CustomErrorResponse
                    {
                        Message     = ResultCode.ArgumentRequired.ToString(),
                        Description = string.Empty,
                        Code        = 400,
                    });
                }

                ResultCode resultCode = await this.SupervisorLog.AddLog(log);

                if (resultCode == ResultCode.Ok)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(405, new CustomErrorResponse
                    {
                        Message     = resultCode.ToString(),
                        Description = string.Empty,
                        Code        = 405,
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new CustomErrorResponse
                {
                    Message     = ex.Message,
                    Description = string.Empty,
                    Code        = 500,
                });
            }
        }
        #endregion
    }
}

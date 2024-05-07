using Framework.Core.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading.Tasks;

namespace Connect.WebApi.Controllers
{
    [Authorize(Policy = "administrator")]
    [Route("connect/[controller]")]
	public class HealthController : Controller
	{
        #region Property
        private HealthCheckService HealthCheckService { get; }
        #endregion

        #region Constructor
        public HealthController(IServiceProvider serviceProvider)
        {
            this.HealthCheckService = serviceProvider.GetRequiredService<HealthCheckService>();
        }
        #endregion

        #region Method

        //ConnectConstants.RestUrlHealthCheck
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            HealthReport? report = await this.HealthCheckService.CheckHealthAsync();
            var reportToJson = report.ToJson();

            if (reportToJson != null)
            {
                return StatusCode(200, reportToJson);
            }
            else
            {
                return NotFound();
            }
        }
        #endregion
    }
}

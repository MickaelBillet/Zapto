using Framework.Core.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WeatherZapto.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableRateLimiting("SlidingPolicy")]
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

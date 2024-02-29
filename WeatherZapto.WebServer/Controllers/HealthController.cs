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

        //WeatherZaptoConstants.UrlHealthCheck
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
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
            catch (HttpRequestException ex)
            {
                return StatusCode(int.Parse(ex.Message));
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

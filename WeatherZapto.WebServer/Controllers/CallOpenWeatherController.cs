using Framework.Core.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WeatherZapto.Data;

namespace WeatherZapto.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableRateLimiting("SlidingPolicy")]
    public class CallOpenWeatherController : Controller
	{
        #region Property
        private ISupervisorCall Supervisor { get; }
        #endregion

        #region Constructor
        public CallOpenWeatherController(IServiceProvider serviceProvider)
        {
            this.Supervisor = serviceProvider.GetRequiredService<ISupervisorCall>();
        }
        #endregion

        #region Method
        [HttpGet("date={date}")]
        public async Task<IActionResult> GetDayCount(string date)
        {
            long? count = null;
            try
            {
                string[] tab = date.Split("-");
                if ((tab != null) && (tab.Length > 0))
                {
                    count = await this.Supervisor.GetDayCallsCount(new DateOnly(int.Parse(tab[2]), int.Parse(tab[1]), int.Parse(tab[0])));
                    if (count != null)
                    {
                        return StatusCode(200, count);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest();
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

        [HttpGet("last30days")]
        public async Task<IActionResult> GetLast30DaysCount()
        {
            long? count = null;
            try
            {
                count = await this.Supervisor.GetLast30DaysCallsCount();
                if (count != null)
                {
                    return StatusCode(200, count);
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

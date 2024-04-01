using Framework.Core.Base;
using Microsoft.AspNetCore.Mvc;
using WeatherZapto.Data;

namespace WeatherZapto.WebServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TemperatureController : Controller
    {
        #region Properties
        private ISupervisorTemperature Supervisor { get; }
        #endregion

        #region Constructor
        public TemperatureController(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.Supervisor = serviceProvider.GetRequiredService<ISupervisorTemperature>();
        }
        #endregion

        #region Methods 
        //WeatherZaptoConstants.UrlTemperaturesDay
        [HttpGet()]
        public async Task<IActionResult> GetTemperature(string location, DateTime date)
        {
            IEnumerable<double>? temperatures = null;

            try
            {
                temperatures = await this.Supervisor.GetTemperatures(location, date);
                if (temperatures != null)
                {
                    return StatusCode(200, temperatures);
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
                return BadRequest(new CustomErrorResponse
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
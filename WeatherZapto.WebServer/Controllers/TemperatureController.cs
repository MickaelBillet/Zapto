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
        [HttpGet("data/date={date}&location={location}")]
        public async Task<IActionResult> GetTemperatures(string date, string location)
        {
            IEnumerable<double>? temperatures = null;

            try
            {
                string[] tab = date.Split("-");
                if ((tab != null) && (tab.Length > 0))
                {
                    temperatures = await this.Supervisor.GetTemperatures(location, new DateTime(int.Parse(tab[2]), int.Parse(tab[1]), int.Parse(tab[0])));
                    if (temperatures != null)
                    {
                        return StatusCode(200, temperatures);
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
        #endregion
    }
}
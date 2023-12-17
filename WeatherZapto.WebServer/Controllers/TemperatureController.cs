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
        [HttpGet("data/date={date}&location={location}")]
        public async Task<IActionResult> GetTemperatures(string date, string location)
        {
            IEnumerable<float>? temperatures = null;

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

        [HttpGet("minmax/date={date}&location={location}")]
        public async Task<IActionResult> GetTemperatureMinMax(string date, string location)
        {
            float?[] minmax = new float?[2];
            try
            {
                string[] tab = date.Split("-");
                if ((tab != null) && (tab.Length > 0))
                {
                    minmax[0] = await this.Supervisor.GetTemperatureMin(location, new DateTime(int.Parse(tab[2]), int.Parse(tab[1]), int.Parse(tab[0])));
                    minmax[1] = await this.Supervisor.GetTemperatureMax(location, new DateTime(int.Parse(tab[2]), int.Parse(tab[1]), int.Parse(tab[0])));

                    return StatusCode(200, minmax);
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
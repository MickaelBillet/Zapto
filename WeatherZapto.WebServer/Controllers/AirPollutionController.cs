using Framework.Core.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WeatherZapto.Application;
using WeatherZapto.Model;

namespace WeatherZapto.WebServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableRateLimiting("SlidingPolicy")]
    public class AirPollutionController : Controller
    {
        #region Properties
        private IApplicationOWServiceCache? ApplicationOWServiceCache { get; }
        private IApplicationOWService? ApplicationOWService { get; }
        private IConfiguration? Configuration { get; }
        #endregion

        #region Constructor
        public AirPollutionController(IServiceProvider serviceProvider, IConfiguration configuration) 
        {
            this.ApplicationOWServiceCache = serviceProvider.GetRequiredService<IApplicationOWServiceCache>();
            this.ApplicationOWService = serviceProvider.GetRequiredService<IApplicationOWService>();
            this.Configuration = configuration;
        }
        #endregion

        #region Methods 
        [HttpGet(@"lon={longitude}&lat={latitude}")]
        public async Task<IActionResult> GetAirPollution(string longitude, string latitude)
        {
            ZaptoAirPollution? zaptoAirPollution = null;

            try
            {
                if ((this.Configuration != null) && (this.ApplicationOWService != null) && (this.ApplicationOWServiceCache != null))
                {
                    ZaptoLocation zaptoLocation = await this.ApplicationOWService.GetReverseLocation(this.Configuration["OpenWeatherAPIKey"], longitude, latitude);
                    if (zaptoLocation != null)
                    {
                        zaptoAirPollution = await this.ApplicationOWServiceCache.GetCurrentAirPollution(this.Configuration["OpenWeatherAPIKey"], zaptoLocation.Location, longitude, latitude);
                        if (zaptoAirPollution != null)
                        {
                            return StatusCode(200, zaptoAirPollution);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    else
                    {
                        return Problem("Location cannnot be found", null, 500, "Server Error");
                    }
                }
                else
                {
                    return Problem("Services not configured", null, 500, "Server Error");
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

        [HttpGet(@"location={location}&lon={longitude}&lat={latitude}")]
        public async Task<IActionResult> GetAirPollution(string location, string longitude, string latitude)
        {
            ZaptoAirPollution? zaptoAirPollution = null;

            try
            {
                if ((this.ApplicationOWService != null) && (this.Configuration != null) && (this.ApplicationOWServiceCache != null))
                {
                    zaptoAirPollution = await this.ApplicationOWServiceCache.GetCurrentAirPollution(this.Configuration["OpenWeatherAPIKey"], location, longitude, latitude);
                    if (zaptoAirPollution != null)
                    {
                        return StatusCode(200, zaptoAirPollution);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return Problem("Services not configured", null, 500, "Server Error");
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

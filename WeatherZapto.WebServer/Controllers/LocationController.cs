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
    public class LocationController : Controller
    {
        #region Properties
        private IApplicationOWService? ApplicationOWService { get; }
        private IConfiguration? Configuration { get; }
        #endregion

        #region Constructor
        public LocationController(IServiceProvider serviceProvider, IConfiguration configuration) 
        {
            this.ApplicationOWService = serviceProvider.GetRequiredService<IApplicationOWService>();
            this.Configuration = configuration;
        }
        #endregion

        #region Methods 
        [HttpGet("lon={longitude}&lat={latitude}")]
        public async Task<IActionResult> GetLocation(string longitude, string latitude)
        {
            ZaptoLocation? zaptoLocation = null;
            try
            {
                if ((this.Configuration != null) && (this.ApplicationOWService != null))
                {
                    zaptoLocation = await this.ApplicationOWService.GetLocation(this.Configuration["OpenWeatherAPIKey"], longitude, latitude);
                    if (zaptoLocation != null)
                    {
                        return StatusCode(200, zaptoLocation);
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

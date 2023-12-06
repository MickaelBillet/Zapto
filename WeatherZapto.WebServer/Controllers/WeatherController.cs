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
    public class WeatherController : Controller
    {
        #region Properties
        private IApplicationOWServiceCache? ApplicationOWServiceCache { get; }
        private IApplicationOWService? ApplicationOWService { get; }
        private IConfiguration? Configuration { get; }
        #endregion

        #region Constructor
        public WeatherController(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.ApplicationOWServiceCache = serviceProvider.GetRequiredService<IApplicationOWServiceCache>();
            this.ApplicationOWService = serviceProvider.GetRequiredService<IApplicationOWService>(); 
            this.Configuration = configuration;
        }
        #endregion

        #region Methods 
        [HttpGet("lon={longitude}&lat={latitude}&cul={culture}")]
        public async Task<IActionResult> GetWeather(string longitude, string latitude, string culture)
        {
            ZaptoWeather? zaptoWeather = null;

            try
            {
                if ((this.Configuration != null) && (this.ApplicationOWService != null) && (this.ApplicationOWServiceCache != null))
                {
                    ZaptoLocation zaptoLocation = await this.ApplicationOWService.GetReverseLocation(this.Configuration["OpenWeatherAPIKey"], longitude, latitude);
                    if (zaptoLocation != null)
                    {
                        zaptoWeather = await this.ApplicationOWServiceCache.GetCurrentWeather(this.Configuration["OpenWeatherAPIKey"], zaptoLocation.Location, longitude, latitude, culture.Substring(0,2));
                        if (zaptoWeather != null)
                        {
                            return StatusCode(200, zaptoWeather);
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

        [HttpGet("location={location}&lon={longitude}&lat={latitude}&cul={culture}")]
        public async Task<IActionResult> GetWeatherLocation(string location, string longitude, string latitude, string culture)
        {
            ZaptoWeather? zaptoWeather = null;

            try
            {
                if ((this.Configuration != null) && (this.ApplicationOWService != null) && (this.ApplicationOWServiceCache != null))
                {
                    zaptoWeather = await this.ApplicationOWServiceCache.GetCurrentWeather(this.Configuration["OpenWeatherAPIKey"], location, longitude, latitude, culture.Substring(0,2));
                    if (zaptoWeather != null)
                    {
                        return StatusCode(200, zaptoWeather);
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
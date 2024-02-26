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
        //UrlReverseLocation
        [HttpGet("lon={longitude}&lat={latitude}")]
        public async Task<IActionResult> GetReverseLocation(string longitude, string latitude)
        {
            ZaptoLocation? zaptoLocation = null;
            try
            {
                if ((this.Configuration != null) && (this.ApplicationOWService != null))
                {
                    zaptoLocation = await this.ApplicationOWService.GetReverseLocation(this.Configuration["OpenWeatherAPIKey"], longitude, latitude);
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

        //UrlLocationCity1
        //UrlLocationCity2
        [HttpGet("city={city}")]
        [HttpGet("city={city}&country={country}")]
        public async Task<IActionResult> GetLocationFromCity(string city, string? state, string? country)
        {
            IEnumerable<ZaptoLocation>? zaptoLocations = null;
            try
            {
                if ((this.Configuration != null) && (this.ApplicationOWService != null))
                {
                    zaptoLocations = await this.ApplicationOWService.GetLocationFromCity(this.Configuration["OpenWeatherAPIKey"], city, state, country);
                    if (zaptoLocations != null)
                    {
                        return StatusCode(200, zaptoLocations);
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

        //WeatherZaptoConstants.UrlLocationZipCode
        [HttpGet("zip={zipcode}&country={country}")]
        public async Task<IActionResult> GetLocationFromZipCode(string zipcode, string country)
        {
            ZaptoLocation? zaptoLocation = null;
            try
            {
                if ((this.Configuration != null) && (this.ApplicationOWService != null))
                {
                    zaptoLocation = await this.ApplicationOWService.GetLocationFromZipCode(this.Configuration["OpenWeatherAPIKey"], zipcode, country);
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
            catch(HttpRequestException ex)
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

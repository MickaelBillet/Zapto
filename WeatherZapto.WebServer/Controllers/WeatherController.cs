using Framework.Core.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WeatherZapto.Application;
using WeatherZapto.Data;
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
        protected IServiceScopeFactory? ServiceScopeFactory { get; set; }
        #endregion

        #region Constructor
        public WeatherController(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.ApplicationOWServiceCache = serviceProvider.GetRequiredService<IApplicationOWServiceCache>();
            this.ApplicationOWService = serviceProvider.GetRequiredService<IApplicationOWService>();
            this.ServiceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            this.Configuration = configuration;
        }
        #endregion

        #region Methods 
        //WeatherZaptoConstants.UrlWeather
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
                            (zaptoWeather.TemperatureMin, zaptoWeather.TemperatureMax) = await this.GetTemperatureMinMax(zaptoLocation?.Location);
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

        //WeatherZaptoConstants.UrlWeatherLocation
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
                        (zaptoWeather.TemperatureMin, zaptoWeather.TemperatureMax) = await this.GetTemperatureMinMax(location);
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

        private async Task<(string min, string max)> GetTemperatureMinMax(string? location)
        {
            string min = string.Empty, max = string.Empty;
            using (IServiceScope scope = this.ServiceScopeFactory!.CreateScope())
            {
                ISupervisorTemperature? supervisorTemperature = scope.ServiceProvider.GetService<ISupervisorTemperature>();
                if ((supervisorTemperature != null) && (string.IsNullOrEmpty(location) == false))
                {
                    double? valmin = await supervisorTemperature.GetTemperatureMin(location, Clock.Now.Date);
                    double? valmax = await supervisorTemperature.GetTemperatureMax(location, Clock.Now.Date);
                    min = valmin != null ? valmin.Value.ToString("0.0") : string.Empty;
                    max=  valmax != null ? valmax.Value.ToString("0.0") : string.Empty;
                }
            }
            return (min, max);
        }
        #endregion
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WeatherZapto.Data;

namespace WeatherZapto.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableRateLimiting("SlidingPolicy")]
    public class CallOWController : Controller
	{
        #region Property
        private ISupervisorCall SupervisorCall { get; }
        #endregion

        #region Constructor
        public CallOWController(IServiceProvider serviceProvider)
        {
            this.SupervisorCall = serviceProvider.GetRequiredService<ISupervisorCall>();
        }
        #endregion

        #region Method
        //[HttpGet("day")]
        //public async Task<IActionResult> GetDayCount()
        //{
            

        //}


        #endregion
    }
}

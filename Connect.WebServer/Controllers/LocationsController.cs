using Connect.Application.Infrastructure;
using Connect.Data;
using Connect.Model;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.WebApi.Controllers
{
    [Authorize(Policy = "user")]
    [Route("connect/[controller]")]
    public class LocationsController : Controller
    {
        #region Property
        ISupervisorLocation SupervisorLocation { get; }
        ISupervisorRoom SupervisorRoom { get; }
        private IAlertService? AlertService { get; }
        #endregion

        #region Constructor
        public LocationsController(IServiceProvider serviceProvider)
        {
            this.SupervisorLocation = serviceProvider.GetRequiredService<ISupervisorLocation>();
            this.SupervisorRoom = serviceProvider.GetRequiredService<ISupervisorRoom>();
            this.AlertService = AlertServiceFactory.CreateAlerteService(serviceProvider, ServiceAlertType.Mail);
        }
        #endregion

        #region Method

        // POST connect/location/test
        [HttpPost("~/connect/Locations/test")]
        public async Task<IActionResult> Post([FromBody] string locationId)
        {
            try
            {
                if (locationId == null || !ModelState.IsValid)
                {
                    return BadRequest(new CustomErrorResponse
                    {
                        Message = ResultCode.ArgumentRequired.ToString(),
                        Description = string.Empty,
                        Code = 400,
                    });
                }

                if (this.AlertService != null)
                {
                    await this.AlertService.SendAlertAsync(locationId, "Coucou", "Location " + locationId);
                    return Ok();
                }
                else
                {
                    return StatusCode(500, new CustomErrorResponse
                    {
                        Message = "Test Error",
                        Description = string.Empty,
                        Code = 500,
                    });
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

        // GET connect/locations
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<Location>? locations;

            try
            {
                var claims = User.Claims;

                locations = await this.SupervisorLocation.GetLocations();
                if (locations != null)
                {
                    return StatusCode(200, locations);
                }
                else
                {
                    return NotFound();
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

        // GET connect/locations/5/rooms
        [HttpGet("~/connect/Locations/{id}/Rooms")]
        public async Task<IActionResult> GetRooms(string id)
		{
            IEnumerable<Room> rooms;
            try
            {
                rooms = await this.SupervisorRoom.GetRooms(id);
                if (rooms != null)
                {
                    return StatusCode(200, rooms);
                }
                else
                {
                    return NotFound();
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

        // GET connect/locations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Location? location; 

            try
            {
                location = await this.SupervisorLocation.GetLocation(id);
                if (location != null)
                {
                    return StatusCode(200, location);
                }
                else
                {
                    return NotFound();
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

        [HttpGet("unauthorized")]
        public IActionResult UnauthorizedTestAction() =>Ok(new { Message = "Access is allowed for unauthorized users" });

        #endregion
    }
}

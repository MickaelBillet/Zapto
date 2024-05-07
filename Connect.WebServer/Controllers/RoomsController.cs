using Connect.Data;
using Connect.Model;
using Framework.Core.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Connect.WebApi.Controllers
{
    [Route("connect/[controller]")]
    [Authorize(Policy = "user")]
    public class RoomsController : Controller
    {
        #region Property

        ISupervisorRoom SupervisorRoom { get; }

        #endregion

        #region Constructor
        public RoomsController(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.SupervisorRoom = serviceProvider.GetRequiredService<ISupervisorFactoryRoom>().CreateSupervisor(byte.Parse(configuration["Cache"]!));
        }
        #endregion

        #region Method

        //ConnectConstants.RestUrlRoomsId
        //GET connect/rooms/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Room? room;

            try
            {
                room = await this.SupervisorRoom.GetRoom(id);

                if (room != null)
                {
                    return StatusCode(200, room);
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

        #endregion
    }
}

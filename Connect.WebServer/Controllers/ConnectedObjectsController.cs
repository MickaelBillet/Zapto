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
    [Authorize(Policy = "user")]
    [Route("connect/[controller]")]
    public class ConnectedObjectsController : Controller
    {
        #region Property

        private ISupervisorConnectedObject SupervisorConnectedObject { get; }

        #endregion

        #region Constructor

        public ConnectedObjectsController(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.SupervisorConnectedObject = serviceProvider.GetRequiredService<ISupervisorFactoryConnectedObject>().CreateSupervisor(int.Parse(configuration["Cache"]!));
        }

        #endregion

        #region HttpRequest

        //ConnectConstants.RestUrlConnectedObjectId
        //GET connect/connectedobjects/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            ConnectedObject? obj;

            try
            {
                obj = await this.SupervisorConnectedObject.GetConnectedObject(id);

                if (obj != null)
                {
                    return StatusCode(200, obj);
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

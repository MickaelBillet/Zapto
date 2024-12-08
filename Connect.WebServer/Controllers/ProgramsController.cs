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
    public class ProgramsController : Controller
    {
        #region Property

        private ISupervisorProgram SupervisorProgram { get; set; }

        #endregion

        #region Constructor

        public ProgramsController(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.SupervisorProgram = serviceProvider.GetRequiredService<ISupervisorFactoryProgram>().CreateSupervisor(int.Parse(configuration["Cache"]!));
        }

        #endregion

        #region Method

        //ConnectConstants.RestUrlProgramsId
        // GET connect/programs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Program? program;

            try
            {
                program = await this.SupervisorProgram.GetProgram(id);

                if (program != null)
                {
                    return StatusCode(200, program);
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

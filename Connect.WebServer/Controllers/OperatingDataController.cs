using Connect.Data;
using Connect.Model;
using Framework.Core.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.WebApi.Controllers
{
    [Route("connect/[controller]")]
    [Authorize(Policy = "user")]
    public class OperatingDataController : Controller
    {
        #region Property

        private ISupervisorOperatingData SupervisorOperatingData { get; }
        #endregion

        #region Constructor

        public OperatingDataController(IServiceProvider serviceProvider)
        {
            this.SupervisorOperatingData = serviceProvider.GetRequiredService<ISupervisorOperatingData>();
        }

        #endregion

        #region Method

        [HttpGet("~/connect/data/date/{date}/rooms/{roomId}")]
        public async Task<IActionResult> GetDataOfRoom(string date, string roomId)
        {
            IEnumerable<OperatingData>? data;

            try
            {
                string[] tab = date.Split("-");
                if ((tab != null) && (tab.Length > 0))
                {
                    data = await this.SupervisorOperatingData.GetRoomOperatingDataOfDay(roomId, new DateTime(int.Parse(tab[2]), int.Parse(tab[1]), int.Parse(tab[0])));
                    if (data != null)
                    {
                        return StatusCode(200, data);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest();
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

        [HttpGet("~/connect/data/rooms/{roomId}/datemax/")]
        public async Task<IActionResult> GetRoomMaxDate(string roomId)
        {
            DateTime? date = null;

            try
            {
                date = await this.SupervisorOperatingData.GetRoomMaxDate(roomId);
                if (date != null)
                {
                    return StatusCode(200, date);
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

        [HttpGet("~/connect/data/rooms/{roomId}/datemin/")]
        public async Task<IActionResult> GetRoomMinDate(string roomId)
        {
            DateTime? date = null;

            try
            {
                date = await this.SupervisorOperatingData.GetRoomMinDate(roomId);
                if (date != null)
                {
                    return StatusCode(200, date);
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

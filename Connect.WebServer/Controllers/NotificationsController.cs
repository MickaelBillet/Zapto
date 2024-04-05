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
    public class NotificationsController : Controller
    {
        #region Property

        ISupervisorCacheNotification SupervisorNotification { get; }

        #endregion

        #region Constructor

        public NotificationsController(IServiceProvider serviceProvider)
        {
            this.SupervisorNotification = serviceProvider.GetRequiredService<ISupervisorCacheNotification>();
        }

        #endregion

        #region Method

        //ConnectConstants.RestUrlNotifications
        //POST connect/notifications
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Notification notification)
        {
            try
            {
                if (notification == null || !ModelState.IsValid)
                {
                    return BadRequest(new CustomErrorResponse
                    {
                        Message = ResultCode.ArgumentRequired.ToString(),
                        Description = string.Empty,
                        Code = 400,
                    });
                }

                ResultCode resultCode = await this.SupervisorNotification.AddNotification(notification);

                if (resultCode == ResultCode.Ok)
                {                   
                    return Ok();
                }
                else
                {
                    return StatusCode(405, new CustomErrorResponse
                    {
                        Message = resultCode.ToString(),
                        Description = string.Empty,
                        Code = 405,
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

        //ConnectConstants.RestUrlNotificationsId
        //PUT connect/notification/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]Notification notification)
        {
            try
            {
                if (notification == null)
                {
                    return BadRequest(new CustomErrorResponse
                    {
                        Message = ResultCode.ArgumentRequired.ToString(),
                        Description = string.Empty,
                        Code = 400,
                    });
                }
                else if (!ModelState.IsValid)
                {
                    return BadRequest(new CustomErrorResponse
                    {
                        Message = ResultCode.CouldNotUpdateItem.ToString(),
                        Description = string.Empty,
                        Code = 400,
                    });
                }
                else
                {
                    //Update in database the notification
                    ResultCode resultCode = await this.SupervisorNotification.UpdateNotification(id, notification);

                    if (resultCode == ResultCode.Ok)
                    {                        
                        return Ok();
                    }
                    else if (resultCode == ResultCode.ItemNotFound)
                    {
                        return NotFound(new CustomErrorResponse
                        {
                            Message = resultCode.ToString(),
                            Description = string.Empty,
                            Code = 404,
                        });
                    }
                    else
                    {
                        return StatusCode(405, new CustomErrorResponse
                        {
                            Message = resultCode.ToString(),
                            Description = string.Empty,
                            Code = 405,
                        });
                    }
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

        //ConnectConstants.RestUrlNotificationsId
        //DELETE connect/notifications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                Notification notification = await this.SupervisorNotification.GetNotification(id);

                if (notification != null)
                {
                    return StatusCode(200, await this.SupervisorNotification.DeleteNotification(notification));
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

        //ConnectConstants.RestUrlRoomNotifications
        //DELETE connect/rooms/5/notifications
        [HttpDelete("~/connect/Rooms/{id}/Notifications")]
        public async Task<IActionResult> DeleteFromRoom(string id)
        {
            IEnumerable<Notification>? notifications;

            try
            {
                notifications = await this.SupervisorNotification.GetNotificationsFromRoom(id);

                if (notifications != null)
                {
                    ResultCode resultCode = await this.SupervisorNotification.DeleteNotifications(notifications);
                    if (resultCode == ResultCode.Ok)
                    {
                        return StatusCode(200, notifications);
                    }
                    else
                    {
                        return StatusCode(409, new CustomErrorResponse
                        {
                            Message = resultCode.ToString(),
                            Description = string.Empty,
                            Code = 409,
                        });
                    }
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

        //ConnectConstants.RestUrlConnectedObjectNotifications
        //DELETE connect/connectedobjects/5/notifications
        [HttpDelete("~/connect/Connectedobjects/{id}/Notifications")]
        public async Task<IActionResult> DeleteFromConnectedObject(string id)
        {
            IEnumerable<Notification>? notifications;

            try
            {
                notifications = await this.SupervisorNotification.GetNotificationsFromConnectedObject(id);

                if (notifications != null)
                {
                    ResultCode resultCode = await this.SupervisorNotification.DeleteNotifications(notifications);

                    if (resultCode == ResultCode.Ok)
                    {
                        return StatusCode(200, notifications);
                    }
                    else
                    {
                        return StatusCode(409, new CustomErrorResponse
                        {
                            Message = resultCode.ToString(),
                            Description = string.Empty,
                            Code = 409,
                        });
                    }
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

        //ConnectConstants.RestUrlRoomNotifications
        //GET connect/rooms/5/notifications
        [HttpGet("~/connect/Rooms/{id}/Notifications")]
        public async Task<IActionResult> GetFromRoom(string id)
        {
            IEnumerable<Notification>? notifications;

            try
            {
                notifications = await this.SupervisorNotification.GetNotificationsFromRoom(id);

                if (notifications != null)
                {
                    return StatusCode(200, notifications);
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

        //ConnectConstants.RestUrlConnectedObjectNotifications
        //GET connect/connectedobjects/5/notifications
        [HttpGet("~/connect/Connectedobjects/{id}/Notifications")]
        public async Task<IActionResult> GetFromConnectedObject(string id)
        {
            IEnumerable<Notification>? notifications;

            try
            {
                notifications = await this.SupervisorNotification.GetNotificationsFromConnectedObject(id);

                if (notifications != null)
                {
                    return StatusCode(200, notifications);
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

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
    [Authorize(Policy = "user")]
    [Route("connect/[controller]")]
    public class ConditionsController : Controller
    {
        #region Property

        ISupervisorCondition SupervisorCondition { get; }

        #endregion

        #region Constructor

        public ConditionsController(IServiceProvider serviceProvider)
        {
            this.SupervisorCondition = serviceProvider.GetRequiredService<ISupervisorCondition>();
        }

        #endregion

        #region Method

        // GET connect/conditions
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<Condition>? conditions;

            try
            {
                conditions = await this.SupervisorCondition.GetConditions();

                if (conditions != null)
                {
                    return StatusCode(200, conditions);
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

        // GET connect/conditions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Condition? condition;

            try
            {
                condition = await this.SupervisorCondition.GetCondition(id);

                if (condition != null)
                {
                    return StatusCode(200, condition);
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

        // POST connect/conditions
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Condition condition)
        {
            try
            {
                if (condition == null || !ModelState.IsValid)
                {
                    return BadRequest(new CustomErrorResponse
                    {
                        Message = ResultCode.ArgumentRequired.ToString(),
                        Description = string.Empty,
                        Code = 400,
                    });
                }

                ResultCode resultCode = await this.SupervisorCondition.AddCondition(condition);

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

        // PUT connect/conditions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]Condition condition)
        {
            try
            {
                if (condition == null)
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
                    ResultCode resultCode = await this.SupervisorCondition.UpdateCondition(id, condition);

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
            catch(Exception ex)
            {
                return StatusCode(500, new CustomErrorResponse
                {
                    Message = ex.Message,
                    Description = string.Empty,
                    Code = 500,
                });
            }
        }

        // DELETE connect/conditions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(String id)
        {
            try
            {
                Condition condition = await this.SupervisorCondition.GetCondition(id);

                if (condition != null)
                {
                    return StatusCode(200, await this.SupervisorCondition.DeleteCondition(condition));
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

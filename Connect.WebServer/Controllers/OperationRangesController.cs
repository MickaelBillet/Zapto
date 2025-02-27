using Connect.Data;
using Connect.Model;
using Framework.Core.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect.WebApi.Controllers
{
    [Route("connect/[controller]")]
    [Authorize(Policy = "user")]
    public class OperationRangesController : Controller
    {
        #region Property
        private ISupervisorOperationRange SupervisorOperationRange { get; set; }
        #endregion

        #region Constructor
        public OperationRangesController(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.SupervisorOperationRange = serviceProvider.GetRequiredService<ISupervisorFactoryOperationRange>().CreateSupervisor(int.Parse(configuration["Cache"]!));
        }
        #endregion

        #region Method
        //ConnectConstants.RestUrlOperationRanges
        //POST connect/operationranges
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OperationRange operationRange)
        {
            try
            {
                if (operationRange == null || !ModelState.IsValid)
                {
                    return BadRequest(new CustomErrorResponse
                    {
                        Message = ResultCode.ArgumentRequired.ToString(),
                        Description = string.Empty,
                        Code = 400,
                    });
                }

                ResultCode resultCode = await this.SupervisorOperationRange.AddOperationRange(operationRange);

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
                Log.Error(ex.Message);

                return StatusCode(500, new CustomErrorResponse
                {
                    Message = ex.Message,
                    Description = string.Empty,
                    Code = 500,
                });
            }
        }

        //ConnectConstants.RestUrlOperationRangesId
        //DELETE connect/operationranges/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                OperationRange operationRange = await this.SupervisorOperationRange.GetOperationRange(id);

                if (operationRange != null)
                {
                    return StatusCode(200, await this.SupervisorOperationRange.DeleteOperationRange(operationRange));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                return StatusCode(500, new CustomErrorResponse
                {
                    Message = ex.Message,
                    Description = string.Empty,
                    Code = 500,
                });
            }
        }

        //ConnectConstants.RestUrlProgramOperationRanges
        //GET connect/programs/5/operationranges
        [HttpGet("~/connect/Programs/{id}/Operationranges")]
        public async Task<IActionResult> GetFromProgram(string id)
        {
            IEnumerable<OperationRange>? operationRanges;

            try
            {
                operationRanges = await this.SupervisorOperationRange.GetOperationRanges(id);

                if (operationRanges != null)
                {
                    return StatusCode(200, operationRanges);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);

                return StatusCode(500, new CustomErrorResponse
                {
                    Message = ex.Message,
                    Description = string.Empty,
                    Code = 500,
                });
            }
        }

        //ConnectConstants.RestUrlProgramOperationRanges
        //DELETE connect/programs/{id}/operationranges
        [HttpDelete("~/connect/Programs/{id}/Operationranges")]
        public async Task<IActionResult> DeleteFromProgram(string id)
        {
            IEnumerable<OperationRange>? operationRanges;

            try
            {
                operationRanges = await this.SupervisorOperationRange.GetOperationRanges(id);

                if (operationRanges != null)
                {
                    ResultCode resultCode = await this.SupervisorOperationRange.DeleteOperationRanges(operationRanges);

                    if (resultCode == ResultCode.Ok)
                    {
                        return StatusCode(200, operationRanges);
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
                Log.Error(ex.Message);

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

﻿using Framework.Common.Services;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Threading.Tasks;
using IMailService = Framework.Infrastructure.Services.IMailService;

namespace Connect.WebApi.Controllers
{
    [Route("connect/[controller]")]
    [Authorize(Policy = "user")]
    public class MailController : Controller
	{
        #region Property
        private IMailService MailService { get; }
        private ISecretService SecretService { get; }
        #endregion

        #region Constructor

        public MailController(IServiceProvider serviceProvider)
        {
            this.MailService = serviceProvider.GetRequiredService<IMailService>();
            this.SecretService = serviceProvider.GetRequiredService<ISecretService>();
        }

        #endregion

        #region Methods

        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequest mailRequest)
        {
            try
            {
                string password = this.SecretService.GetSecret("MailPassword");
                string address = this.SecretService.GetSecret("MailAddress");
                await this.MailService.SendEmailAsync(mailRequest, MailSent, password, address); 
                return Ok();
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

        private void MailSent(object? sender, MessageSentEventArgs arg)
        {
            Log.Information("Mail Sent");
        }

        #endregion
    }
}

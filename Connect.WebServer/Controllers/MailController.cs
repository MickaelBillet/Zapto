using Framework.Common.Services;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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
        private IKeyVaultService KeyVaultService { get; }
        #endregion

        #region Constructor

        public MailController(IServiceProvider serviceProvider)
        {
            this.MailService = serviceProvider.GetRequiredService<IMailService>();
            this.KeyVaultService = serviceProvider.GetRequiredService<IKeyVaultService>();
        }

        #endregion

        #region Methods

        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {
            try
            {
                string password = this.KeyVaultService.GetSecret("MailPassword");
                await this.MailService.SendEmailAsync(request, MailSent, password);
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
        }

        #endregion
    }
}

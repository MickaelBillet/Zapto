using Framework.Common.Services;
using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.WebServer.Services
{
    internal class SendMessageToArduinoService : ISendMessageToArduinoService
    {
        #region Properties
        private ISendMessageService? SendMessageService { get; }       
        #endregion

        #region Constructor
        public SendMessageToArduinoService(IServiceProvider serviceProvider, string type) 
        {
            if (type == CommunicationType.WebSocket)
            {
                this.SendMessageService = serviceProvider.GetRequiredService<IWSMessageManager>();
            }
            else if (type == CommunicationType.Serial)
            {
                this.SendMessageService = serviceProvider.GetRequiredService<ISerialCommunicationService>();
            }
        }
        #endregion

        #region Methods
        public async Task<bool> SendMessageAsync(string message)
        {
            return (this.SendMessageService != null) ? await this.SendMessageService.SendMessageAsync(message) : await Task.FromResult<bool>(false);
        }
        #endregion
    }
}

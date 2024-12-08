using Framework.Common.Services;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Connect.WebServer.Services
{
    internal class SendMessageToArduinoService : ISendMessageToArduinoService
    {
        #region Properties
        private IWSMessageManager? WSMessageManager { get; }
        #endregion

        #region Constructor
        public SendMessageToArduinoService(IServiceProvider serviceProvider) 
        {
            this.WSMessageManager = serviceProvider.GetRequiredService<IWSMessageManager>();
        }
        #endregion

        #region Methods
        public async Task<int> Send(string json)
        {
            return (this.WSMessageManager != null) ? await this.WSMessageManager.SendMessageToAllAsync(json) : await Task.FromResult<int>(-1);
        }
        #endregion
    }
}

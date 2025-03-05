using Framework.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO.Ports;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public class SendSerialCommunicationService : ISerialCommunicationService
    {
        private readonly ISerialPortService _serialPortService;

        #region Constructor
        public SendSerialCommunicationService(IServiceProvider serviceProvider)
        {
            _serialPortService = serviceProvider.GetRequiredService<ISerialPortService>();
        }
        #endregion

        #region Methods

        public void Dispose()
        {

        }

        public async Task<bool> SendMessageAsync(string message)
        {
            bool res = true;
            try
            {
                await Task.Run(() =>
                {
                    SerialPort serialPort = _serialPortService.GetSerialPort();
                    if ((serialPort != null) && (serialPort.IsOpen == true))
                    {
                        serialPort.WriteLine(message);
                        Log.Information($"Envoyé vers Arduino: {message}");
                    }
                    else
                    {
                        Log.Error("Erreur lors de l'envoi des données sur le port série");
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                res = false;
            }
            return res;
        }
        #endregion
    }
}

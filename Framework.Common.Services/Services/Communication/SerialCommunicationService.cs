using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO.Ports;
using System.Threading.Tasks;

namespace Framework.Common.Services
{
    public class SerialCommunicationService : ScheduledService
    {
        #region Constructor
        public SerialCommunicationService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base(serviceScopeFactory, 200)
        {
            
        }
        #endregion

        #region Methods
        public override async Task ProcessInScope(IServiceScope scope)
        {
            try
            {
                IEventBusProducerConnect eventBusProducer = scope.ServiceProvider.GetRequiredService<IEventBusProducerConnect>();
                ISerialPortService serialPortService = scope.ServiceProvider.GetRequiredService<ISerialPortService>();
                SerialPort serialPort = serialPortService.GetSerialPort();

                if (serialPort != null)
                {
                    if (serialPort.IsOpen == false)
                    {
                        serialPort.Open();
                    }

                    if (serialPort.IsOpen == true)
                    {
                        string buffer = "";
                        while (true)
                        {
                            string data = serialPort.ReadExisting();
                            if (!string.IsNullOrEmpty(data))
                            {
                                buffer += data;
                                int index = buffer.IndexOf("\n");
                                while (index != -1)
                                {
                                    string completeMessage = buffer.Substring(0, index).Trim();
                                    buffer = buffer.Substring(index + 1);
                                    Log.Information($"Reçu d'Arduino: {completeMessage}");
                                    MessageArduino messageArduino = MessageArduino.Deserialize(completeMessage);
                                    await eventBusProducer.Publish(messageArduino);
                                    index = buffer.IndexOf("\n");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erreur lors de l'ouverture du port série.");
            }
            finally
            {

            }
        }
        #endregion
    }
}

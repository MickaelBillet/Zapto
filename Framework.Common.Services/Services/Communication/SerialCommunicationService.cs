using Framework.Core.Base;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO.Ports;
using System.Threading.Tasks;

namespace Framework.Common.Services
{
    public class SerialCommunicationService : IDisposable, ISerialCommunicationService
    {
        private readonly IEventBusProducerConnect _eventBusProducer;
        private readonly string _portName;
        private readonly int _baudRate;

        #region Constructor
        public SerialCommunicationService(IServiceProvider serviceProvider, string portName, int baudRate)
        {
            _eventBusProducer = serviceProvider.GetRequiredService<IEventBusProducerConnect>();
            _portName = portName;
            _baudRate = baudRate;
        }
        #endregion

        #region Methods
        public bool Receive()
        {
            bool res = true;
            string buffer = "";

            try
            {
                using (SerialPort serialPort = new SerialPort(_portName, _baudRate))
                {
                    serialPort.NewLine = "\n";
                    serialPort.ReadTimeout = 500;
                    serialPort.DataReceived += async (sender, e) =>
                    {
                        buffer += serialPort.ReadExisting();
                        int index = buffer.IndexOf("\n");
                        while (index != -1)
                        {
                            string completeMessage = buffer.Substring(0, index).Trim();
                            buffer = buffer.Substring(index + 1);
                            Console.WriteLine($"Message Arduino: {completeMessage}");
                            MessageArduino messageArduino = MessageArduino.Deserialize(completeMessage);
                            await _eventBusProducer.Publish(messageArduino);
                            index = buffer.IndexOf("\n");
                        }
                    };
                    serialPort.Open();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                res = false;
            }           
            return res;
        }

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
                    using (SerialPort serialPort = new SerialPort(_portName, _baudRate))
                    {
                        serialPort.NewLine = "\n";
                        serialPort.Open();
                        if (serialPort.IsOpen)
                        {
                            serialPort.WriteLine(message);
                        }
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

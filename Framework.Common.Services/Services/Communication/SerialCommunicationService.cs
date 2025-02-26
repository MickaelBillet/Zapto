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
        private readonly SerialPort _serialPort;

        #region Constructor
        public SerialCommunicationService(IServiceProvider serviceProvider, string portName, int baudRate)
        {
            _eventBusProducer = serviceProvider.GetRequiredService<IEventBusProducerConnect>();
            _serialPort = new SerialPort(portName, baudRate);
        }
        #endregion

        #region Methods
        public bool Receive()
        {
            bool res = true;
            string buffer = "";

            try
            {
                _serialPort.NewLine = "\n";
                _serialPort.DataReceived += async (sender, e) =>
                {
                    SerialPort sp = (SerialPort)sender;
                    buffer += sp.ReadExisting();
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
                _serialPort.Open();
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
            _serialPort.Close();
            _serialPort.Dispose();
        }

        public async Task<bool> SendMessageAsync(string message)
        {
            bool res = true;
            try
            {
                _serialPort.NewLine = "\n";
                _serialPort.Open();
                if (_serialPort.IsOpen)
                {
                    _serialPort.WriteLine(message);
                    await Task.Delay(100);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                res = false;
            }
            finally
            {
                _serialPort.Close();
            }
            return res;
        }
        #endregion
    }
}

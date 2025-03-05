using System.IO.Ports;

namespace Framework.Common.Services
{
    public class SerialPortService : ISerialPortService
    {
        private readonly SerialPort _serialPort;

        #region Constructor
        public SerialPortService(int baudRate, string portName)
        {
            _serialPort = new SerialPort(portName, baudRate);
            _serialPort.NewLine = "\n";
        }
        #endregion

        #region Methods
        public SerialPort GetSerialPort()
        {
            return _serialPort;
        }
        #endregion
    }
}

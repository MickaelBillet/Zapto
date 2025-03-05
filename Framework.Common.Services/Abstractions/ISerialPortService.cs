using System.IO.Ports;

namespace Framework.Common.Services
{
    public interface ISerialPortService
    {
        SerialPort GetSerialPort();
    }
}
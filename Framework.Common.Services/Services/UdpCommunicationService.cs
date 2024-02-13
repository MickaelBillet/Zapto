using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    internal class UdpCommunicationService : IUdpCommunicationService
    {
        #region Constructor

        public UdpCommunicationService()
        {
        }

        #endregion

        #region Methods

        public async Task<byte[]> StartReception(int port)
        {
            using (UdpClient udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, port)))
            {
                UdpReceiveResult udpReceiveResult = await udpClient.ReceiveAsync();

                return udpReceiveResult.Buffer;
            }                
        }

        public async Task<int> SendMessage(string message, int port, string address)
        {
            int res = 0;

            if (address != null)
            {
                using (UdpClient udpClient = new UdpClient())
                {
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(address), port);

                    // Sends a message to the host to which you have connected.
                    byte[] sendBytes = Encoding.ASCII.GetBytes(message);

                    res = await udpClient.SendAsync(sendBytes, sendBytes.Length, remoteEP);
                }
            }

            return res;
        }

        #endregion
    }
}

using System;
using System.Text;

namespace Framework.Core.Base
{
	public class MessageArduino
    {
        #region Property

        public int Header
        {
            get; set;
        }

        public string Payload
        {
            get; set;
        }

		#endregion

		#region Methods
		public static MessageArduino Deserialize(byte[] buffer, int count)
        {
            string[] delimiter = { "Hdr:", "-Pld:" };

            string received = Encoding.ASCII.GetString(buffer, 0, count).TrimEnd('\0');

            MessageArduino messageArduino = new MessageArduino()
            {
                Header = short.Parse(received.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)[0]),
                Payload = received.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)[1],
            };

            return messageArduino;
        }

        public static string Serialize(MessageArduino msg)
		{
            return $"Hdr:{msg.Header}-Pld:{msg.Payload}";
		}
		#endregion
	}
}

using System;

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
		public static MessageArduino Deserialize(string msg)
        {
            string[] delimiter = { "Hdr:", "-Pld:" };

            MessageArduino messageArduino = new MessageArduino()
            {
                Header = short.Parse(msg.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)[0]),
                Payload = msg.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)[1],
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

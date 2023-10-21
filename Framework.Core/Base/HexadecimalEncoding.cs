using System;
using System.Text;

namespace Framework.Core.Base
{
    public static class HexadecimalEncoding
    {
        public static string ToHexString(string str)
        {
            StringBuilder sb = new StringBuilder();

            byte[] bytes = Encoding.ASCII.GetBytes(str);

            foreach (byte t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString(); 
        }

        public static string FromHexString(string hexString)
        {
            byte[] bytes = new byte[hexString.Length / 2];

            for (byte i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.ASCII.GetString(bytes); 
        }
    }
}

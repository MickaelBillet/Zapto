using System;

namespace Framework.Core.Base
{
    public static class ByteArrayExtension
    {
        public static byte[] Truncate(this byte[] byteArray, int startIndex, int length)
        {
            byte[] result = new byte[length];

            Array.Copy(byteArray, startIndex, result, 0, length);

            return result;
        }
    }
}

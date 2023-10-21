namespace Framework.Core.Base
{
    public static class BCDConverter
    {
        /// <summary>
        /// Convertir un BCD en décimal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ConvertBin2BCD(byte value)
        {
            return (byte)((byte)(value / 10) * 16 + (value % 10));
        }

        /// <summary>
        /// Convertir un décimal en BCD
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ConvertBCD2Bin(byte value)
        {
            return (byte)((byte)(value / 16) * 10 + (value & 15));
        }
    }
}

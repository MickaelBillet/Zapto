namespace Framework.Core.Base
{
    public static class CRC16_MSP430
    {
        public static long Calcul_CRC16MSP430(int crcref, string buf, int len)
        {
            int crc;
            int tmp, crc_new;

            //init crc
            crc = crcref;

            for (int i = 0; i < len; i++)
            {
                tmp = (ushort)buf[i];

                crc_new = ((crc >> 8) | (crc << 8)) & 0xffff;
                crc_new ^= tmp;
                crc_new ^= (char)(crc_new & 0xff) >> 4;
                crc_new ^= (crc_new << 12) & 0xffff;
                crc_new ^= ((crc_new & 0xff) << 5) & 0xffff;
                crc = crc_new;
            }

            return (long)(crc);
        }
    }
}

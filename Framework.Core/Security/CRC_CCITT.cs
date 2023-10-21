namespace Framework.Core.Base
{
    public class CRC_CCITT
    {
        #region Proprietes et Attributs

        //Création d'un Singleton
        private static CRC_CCITT instance = null;

        /// <summary>
        /// Propriété de l'attribut instance
        /// </summary>
        public static CRC_CCITT Instance
        {
            get
            {
                //Section critique en assurant le verrouillage par exclusion mutuelle 
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CRC_CCITT();
                    }

                    return instance;
                }
            }
        }

        private static readonly object padlock = new object();

        private readonly int[] m_reverse_tabl = new int[256];
        private readonly int[] m_CRC16_tabl = new int[256];

        #endregion

        #region Constructeur

        private CRC_CCITT()
        {
            const int poly = 0x1021;
            int crc;
            int rv;
            int refbyte;

            int i, j;

            for (i = 0; i < 256; i++)
            {
                crc = i * 256;

                // Init Tableau de CRC
                for (j = 0; j < 8; j++)
                {
                    if ((crc & 0x8000) == 0)
                    {
                        crc *= 2;
                    }
                    else
                    {
                        crc = ((crc * 2) ^ poly) & 0xffff;
                    }
                }

                // Init Tableau de reverse Byte
                rv = 0;
                refbyte = 1;

                for (j = 0; j < 8; j++)
                {
                    rv *= 2;

                    if ((i & refbyte) > 0)
                    {
                        rv += 1;
                    }
                    refbyte *= 2;
                }

                m_CRC16_tabl[i] = crc;
                m_reverse_tabl[i] = rv;
            }
        }
        #endregion

        #region Méthodes
        private int Calc_CRC(int oldCRC, int ch)
        {
            int crc;
            int ctmp;

            crc = oldCRC;
            ctmp = ch & 255;

            ctmp = (ctmp ^ (crc / 256)) & 255;

            crc = ((crc * 256) ^ m_CRC16_tabl[ctmp]) & 65535;

            return crc;
        }

        public int Calc_CRC(byte[] data, int startindex, int length)
        {
            int i;
            int crc;

            crc = 65535;

            if (data.Length > 0)
            {
                for (i = startindex; i < startindex + length; i++)
                {
                    crc = Calc_CRC(crc, m_reverse_tabl[data[i]]);
                    crc = Calc_CRC(crc, '\0');
                }
            }

            return crc;
        }
        #endregion
    }
}

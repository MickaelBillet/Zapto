using System;

namespace Connect.Model
{
    public abstract class Message
    {
        #region Properties

        protected static readonly string CMD_SEPARATOR_MSG = "YYY";

        protected static readonly string CMD_SEPARATOR_ACQ = "YXY";

        protected static readonly string BODY_SEPARATOR = "XXX";

        #endregion

        #region Methods

        public abstract string CreateCmdMessage(Configuration obj, string command);

        public static (Configuration config, string status) GetAcknowledgementMsg(string data)
        {
            int ind0;
            int ind1;
            int ind2;
            int ind3;

            string status = string.Empty;

            Configuration config = new Configuration();

            int size_separator = CMD_SEPARATOR_ACQ.Length;

            if (data.StartsWith(CMD_SEPARATOR_ACQ))
            {
                ind0 = data.IndexOf(BODY_SEPARATOR);  
                config.ProtocolType = data.Substring(size_separator, ind0 - size_separator);

                data = data.Substring(ind0 + size_separator);

                ind1 = data.IndexOf(BODY_SEPARATOR); 
                config.Address = data.Substring(0, ind1);

                data = data.Substring(ind1 + size_separator);

                ind2 = data.IndexOf(BODY_SEPARATOR);
                config.Unit = data.Substring(0, ind2);

                data = data.Substring(ind2 + size_separator);

                ind3 = data.IndexOf(BODY_SEPARATOR);
                status = data.Substring(0, ind3);
            }

            return (config, status);
        }

        #endregion
    }
}

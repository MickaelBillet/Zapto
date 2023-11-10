using Framework.Core.Domain;
using System;

namespace Connect.Model
{
    public class ServerIotStatus : Item
    {
        #region Property
        public DateTime? ConnectionDate
        { 
            get; set; 
        }

        public string? IpAddress
        {
            get; set;
        }
        #endregion
    }
}

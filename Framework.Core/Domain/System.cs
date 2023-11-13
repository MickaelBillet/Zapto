using System;

namespace Framework.Core.Model
{
	public class SystemStatus
	{
        public DateTime Date
        { 
            get; set;
        }

        public string IpAddress
        {
            get; set;
        }

        public int RSSI
        {
            get; set;
        }        

        public string Status
        {
            get; set;
        }
    }
}

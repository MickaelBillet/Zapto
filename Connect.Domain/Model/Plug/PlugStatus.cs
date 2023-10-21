namespace Connect.Model
{
    public struct PlugStatus
    {
        #region Property

        public static readonly string Name = "PlugStatus";

        public double WorkingDuration
        {
            get; set;
        }

        public int Status
        {
            get; set;
        }

        public string PlugId
        {
            get; set;
        }

        public int Order
        {
            get; set;
        }

        public int Mode
		{
            get; set;
		}

        public int OnOff
        {
            get; set;
        }
        #endregion
    }
}

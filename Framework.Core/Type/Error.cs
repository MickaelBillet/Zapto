namespace Framework.Core.Base
{
	public class Error
	{
        #region Properties

        public string Ident { get; set; }

        public bool IsRecorded { get; set; }

        public bool IsNotified { get; set; }

        public string Message { get; set; }


        #endregion

        #region Constructor

        public Error(string message, string ident, bool isNotified, bool isRecorded = false)
        {
            this.Message = message;
            this.Ident = ident;
            this.IsNotified = isNotified;
            this.IsRecorded = isRecorded;
        }

        #endregion
    }
}

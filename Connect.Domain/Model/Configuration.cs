using Framework.Core.Domain;

namespace Connect.Model
{
    public class Configuration : Item
    {
        private string? protocolType = null;
        private int period = -1;
        private int pin0 = -1;
        private string? unit = null;
        private string? address = null;


        #region Property

        public string? Unit
        {
            get { return unit; }

            set { SetProperty<string?>(ref unit, value); }
        }

        public string? Address
        {
            get { return address; }

            set { SetProperty<string?>(ref address, value); }
        }

        public int Period
        {
            get { return period; }

            set { SetProperty<int>(ref period, value); }
        }

        public int Pin0
        {
            get { return pin0; }

            set { SetProperty<int>(ref pin0, value); }
        }

        public string? ProtocolType
        {
            get { return protocolType; }

            set { SetProperty<string?>(ref protocolType, value); }
        }

        #endregion

        #region Constructor

        public Configuration(): base() { }

        #endregion

        #region Method

        #endregion
    }
}

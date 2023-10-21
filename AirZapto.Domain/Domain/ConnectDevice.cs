using Framework.Core.Base;
using Framework.Core.Domain;

namespace AirZapto.Model
{
    public abstract class ConnectDevice : Item
    {
        private int type = DeviceType.None;
        private string? name = string.Empty;
        private int isRunning = RunningStatus.UnHealthy;

        #region Property

        public int IsRunning
        {
            get { return isRunning; }

            set { SetProperty<int>(ref isRunning, value); }
        }

        public string? Name
        {
            get { return name; }

            set { SetProperty<string?>(ref name, value); }
        }

        public int Type
        {
            get { return type; }

            set { SetProperty<int>(ref type, value); }
        }

        #endregion

        #region Constructor

        protected ConnectDevice() : base()
        {

        }

        #endregion

        #region Method

        #endregion
    }
}

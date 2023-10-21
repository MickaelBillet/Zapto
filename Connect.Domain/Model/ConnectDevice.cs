using Framework.Core.Domain;
using System;

namespace Connect.Model
{
    public abstract class ConnectDevice : Item
    {
        private int type = DeviceType.None;
        private double workingDuration = 0;
        private DateTime? lastDateTimeOn = null;
        private string? name = null;

        #region Property

        public string RoomId { get; set; } = string.Empty;

        public string? ConnectedObjectId { get; set; }

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

        public DateTime? LastDateTimeOn
        {
            get { return lastDateTimeOn; }

            set { SetProperty<DateTime?>(ref lastDateTimeOn, value); }
        }

        public double WorkingDuration
        {
            get { return workingDuration; }

            set
            {
                SetProperty<double>(ref workingDuration, value);
            }
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

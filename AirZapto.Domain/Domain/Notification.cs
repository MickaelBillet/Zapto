using Framework.Core.Domain;

namespace AirZapto.Model
{
	public class Notification : Item
    {
        private bool _isEnabled = true;
        private short _parameter = 0;
        private short _value = 0;

        #region Property

        public string? RoomId { get; set; }

        public bool IsEnabled
        {
            get { return _isEnabled; }

            set { SetProperty<bool>(ref _isEnabled, value); }
        }

        public short Parameter
        {
            get { return _parameter; }

            set { SetProperty<short>(ref _parameter, value); }
        }

        public short Value
        {
            get { return _value; }

            set { SetProperty<short>(ref _value, value); }
        }

        #endregion

        #region Constructor

        public Notification() : base()
        {
        }

        #endregion

        #region Method
       
        #endregion
    }
}

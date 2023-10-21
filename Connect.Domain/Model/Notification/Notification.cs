using System.Text.Json.Serialization;
using Framework.Core.Domain;

namespace Connect.Model
{
    public class Notification : Item
    {
        public const byte LIMIT = 1;

        private bool _isEnabled = true;
        private short _parameter = 0;
        private short _sign = 0;
        private short _value = 0;

        #region Property

        public string? RoomId { get; set; }

        public string? ConnectedObjectId { get; set; }

        public bool IsEnabled
        {
            get { return _isEnabled; }

            set { SetProperty(ref _isEnabled, value); }
        }

        public short Parameter
        {
            get { return _parameter; }

            set { SetProperty(ref _parameter, value); }
        }

        public short Sign
        {
            get { return _sign; }

            set { SetProperty(ref _sign, value); }
        }

        public short Value
        {
            get { return _value; }

            set { SetProperty(ref _value, value); }
        }

        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public byte ConfirmationFlag
        {
            get; set;
        }

        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public Notification Self
        {
            get { return this; }
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

using Framework.Core.Domain;

namespace Connect.Model
{
    public class OperatingData : Item
    {
        #region Property

        public string? ConnectedObjectId { get; set; }

        public int? PlugOrder { get; set; }

        public int? PlugStatus { get; set; }

        public double? Temperature { get; set; }

        public double? Humidity { get; set; }

        public double? Pressure { get; set; }

        public double? WorkingDuration { get; set; }

        public string? RoomId { get; set; }

        #endregion

        #region Constructor

        public OperatingData(): base() { }

        #endregion

        #region Method

        #endregion
    }
}

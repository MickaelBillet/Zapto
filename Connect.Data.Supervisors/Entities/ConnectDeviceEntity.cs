using Framework.Core.Data;
using System.ComponentModel.DataAnnotations;

namespace Connect.Data.Entities
{
    public abstract class ConnectDeviceEntity : ItemEntity
    {
        [Required] public string RoomId { get; set; }
        public string ConnectedObjectId { get; set; } = string.Empty;
        [MaxLength(32)] public string Name { get; set; } = string.Empty;
        public int Type { get; set; }
        public DateTime? LastDateTimeOn { get; set; }
        public double WorkingDuration { get; set; }
    }
}

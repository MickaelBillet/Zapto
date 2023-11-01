using Framework.Core.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect.Data.Entities
{
    [Table("Room")]
    public class RoomEntity : ItemEntity
    {
        [Required] public string LocationId { get; set; } = string.Empty;
        public short Type { get; set; }
        [MaxLength(32)] public string Name { get; set; } = string.Empty;
        [MaxLength(32)] public string Description { get; set; } = string.Empty;
        public double? Humidity { get; set; }
        public double? Temperature { get; set; }
        public double? Pressure { get; set; }
        public int DeviceType { get; set; }
    }
}

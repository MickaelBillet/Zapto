using Framework.Core.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect.Data.Entities
{
    [Table("OperatingData")]
    public class OperatingDataEntity : ItemEntity
    {
        public string ConnectedObjectId { get; set; }
        public int? PlugOrder { get; set; }
        public int? PlugStatus { get; set; }
        public double? Temperature { get; set; }
        public double? Humidity { get; set; }
        public double? Pressure { get; set; }
        public double? WorkingDuration { get; set; }
        [Required] public string RoomId { get; set; }
    }
}

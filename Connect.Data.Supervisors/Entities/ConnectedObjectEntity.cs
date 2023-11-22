using Framework.Core.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect.Data.Entities
{
    [Table("ConnectedObject")]
    public class ConnectedObjectEntity : ItemEntity
    {
        [Required] public string RoomId { get; set; }
        public int DeviceType { get; set; }
        [MaxLength(32)] public string Name { get; set; } = string.Empty;
    }
}

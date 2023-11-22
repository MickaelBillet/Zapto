using Framework.Core.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect.Data.Entities
{
    [Table("Notification")]
    public class NotificationEntity : ItemEntity
    {
        [Required] public string RoomId { get; set; }
        [MaxLength(32) ]public string ConnectedObjectId { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
        public short Parameter { get; set; }
        public short Sign { get; set; }
        public short Value { get; set; }
        public byte ConfirmationFlag { get; set; }
    }
}

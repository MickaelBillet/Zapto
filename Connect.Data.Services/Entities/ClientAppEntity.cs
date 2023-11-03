using Framework.Core.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect.Data.Entities
{
    [Table("ClientApp")]
    public class ClientAppEntity : ItemEntity
    {
        [Required] public string LocationId { get; set; } = string.Empty;
        [MaxLength(32)] public string Description { get; set; } = string.Empty;
        [MaxLength(64)] public string Token { get; set; } = string.Empty;
    }
}

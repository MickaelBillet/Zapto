using Framework.Core.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect.Data.Entities
{
    [Table("Location")]
    public class LocationEntity : ItemEntity
    {
        [MaxLength(64)] public string Address { get; set; } = string.Empty;
        [MaxLength(16)] public string City { get; set; } = string.Empty;
        [MaxLength(16)] public string ZipCode { get; set; } = string.Empty;
        [MaxLength(16)] public string Country { get; set; } = string.Empty;
        [MaxLength(32)] public string Description { get; set; } = string.Empty;
        [MaxLength(16)] public string UserId { get; set; } = string.Empty;
    }
}

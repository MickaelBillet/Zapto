using Framework.Core.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect.Data.Entities
{
    [Table("Configuration")]
    public class ConfigurationEntity : ItemEntity
    {
        [MaxLength(32)] public string Unit { get; set; } = string.Empty;
        [MaxLength(64)] public string Address { get; set; } = string.Empty;
        public int Period { get; set; }
        public int Pin0{ get; set; }
        [MaxLength(16)] public string ProtocolType { get; set; } = string.Empty;
    }
}

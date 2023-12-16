using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect.Data.Entities
{
    [Table("Plug")]
    public class PlugEntity : ConnectDeviceEntity
    {
        [MaxLength(32)] public string ConfigurationId { get; set; } = string.Empty;
        [MaxLength(32)] public string ProgramId { get; set; } = string.Empty;
        [MaxLength (32)] public string ConditionId { get; set; } = string.Empty;
        public int Status { get; set; }
        public int OnOff { get; set; }
        public int Order { get; set; }
        public int Mode { get; set; }
        public int ConditionType { get; set; }
        public int LastCommandSent { get; set; } = 0;
        public DateTime? LastCommandDateTime { get; set; } = null;
    }
}


using Framework.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect.Data.Entities
{
    [Table("OperationRange")]
    public class OperationRangeEntity : ItemEntity
    {
        [Required] public string ProgramId { get; set; } = string.Empty;
        [MaxLength(32)] public string ConditionId { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DayOfWeek Day { get; set; }
        public Boolean AllDay { get; set; }
    }
}

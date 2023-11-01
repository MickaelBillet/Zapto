using Framework.Core.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect.Data.Entities
{
    [Table("Condition")]
    public class ConditionEntity : ItemEntity
    {
        [Required] public string ConnectedObjectId { get; set; }
        public string? OperationRangetId { get; set; }
        public float? HumidityOrder { get; set; }
        public float? TemperatureOrder { get; set; }
        public int TemperatureOrderIsEnabled { get; set; }
        public int HumidityOrderIsEnabled { get; set; }
    }
}

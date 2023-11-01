using Framework.Core.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirZapto.Data.Entities
{
    [Table("SensorData")]
	public class SensorDataEntity : ItemEntity
	{
		[ForeignKey("Sensor")] public string SensorId { get; set; } = string.Empty;
		public float? Temperature { get; set; }
		public int? CO2 { get; set; }
	}
}

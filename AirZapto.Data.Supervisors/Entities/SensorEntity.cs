using Framework.Core.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirZapto.Data.Entities
{
    [Table("Sensor")]
	public class SensorEntity : ItemEntity
	{
		public int Port { get; set; }
		[MaxLength(16)] public string IpAddress { get; set; } = string.Empty;
		public string IdSocket { get; set; } = string.Empty;	
		public float OffSetHumidity { get; set; }
		public float OffSetTemperature { get; set; }
		public float? Humidity { get; set; }
		public float? Temperature { get; set; }
		public float? Pressure { get; set; }
		public int IsRunning { get; set; }
		public int? CO2 { get; set; }
		[MaxLength(8)] public string Period { get; set; } = string.Empty;
		[MaxLength(8)] public string Channel { get; set; } = string.Empty;
		[Required]
		public int Type { get; set; }
		[MaxLength(16)] public string Name { get; set; } = string.Empty;
		[MaxLength(32)] public string Description { get; set; } = string.Empty;
		[MaxLength(64)] public string ThresholdCO2 { get; set; } = string.Empty;
		public int Mode { get; set; }
	}
}

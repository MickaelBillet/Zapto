
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect.Data.Entities
{
    [Table("Sensor")]
    public class SensorEntity : ConnectDeviceEntity
    {
        [MaxLength(16)] public string IpAddress { get; set; } = string.Empty;
        public float OffSetHumidity { get; set; }
        public float OffSetTemperature { get; set; }
        public int LeakDetected { get; set; }
        public double? Humidity { get; set; }
        public bool IsRunning { get; set; }
        public double? Temperature { get; set; }
        public double? Pressure { get; set; }
        [MaxLength(16)] public string Period { get; set; } = string.Empty;
        [MaxLength(64)] public string Parameter { get; set; } = string.Empty;
        [MaxLength(16)] public string Channel { get; set; } = string.Empty;
    }
}

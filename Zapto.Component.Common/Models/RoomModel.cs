using Connect.Model;

namespace Zapto.Component.Common.Models
{
    public record RoomModel : BaseModel
	{
		private string? _temperature;
		private string? _humidity;
		public string LocationId { get; set; } = string.Empty;
		public string? LocationName { get; set; } = string.Empty;
		public string? FileNameImage
		{
			get
            {
				if (this.Type == RoomType.Kitchen)
				{
					return @"Images\sofa-clipart.svg";
				}
				else if (this.Type == RoomType.Bathroom)
				{
					return @"Images\bathtub-clipart.svg";
				}
				else if (this.Type == RoomType.Bedroom)
				{
					return @"Images\modern-bed-clipart.svg";
				}
				else if (this.Type == RoomType.SmallBedroom)
				{
					return @"Images\single-bed-clipart.svg";
				}
				else
                {
					return null;
                }
            }
		}
		public short? Type { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public int StatusSensors { get; set; }
		public string? Humidity
		{
			get
			{
				return String.Format("{0} %", _humidity);
			}

			set
			{
				_humidity = value;
			}
		}
		public string? Temperature 
		{ 
			get
			{
				return String.Format("{0} °C", _temperature);
			}
			
			set
			{
				_temperature = value;
			}
		}
		public string? Pressure { get; set; }
		public int? DeviceType { get; set; }
		public ICollection<ConnectedObject>? ConnectedObjectsList { get; set; }
		public ICollection<Sensor>? Sensors { get; set; }
	}
}

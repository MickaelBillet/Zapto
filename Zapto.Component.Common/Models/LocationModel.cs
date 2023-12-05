namespace Zapto.Component.Common.Models
{
    public class LocationModel : BaseModel
	{
        public string? Location { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public bool LocalizationIsAvailable { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Zip))
            {
                return $"{Location} - {State} - {Country} -  {Latitude!.Value.ToString("F3")} - {Longitude!.Value.ToString("F3")}";
            }
            else
            {
                return $"{Location} - {Zip} - {Country} -  {Latitude!.Value.ToString("F3")} - {Longitude!.Value.ToString("F3")}";
            }
        }
    }
}

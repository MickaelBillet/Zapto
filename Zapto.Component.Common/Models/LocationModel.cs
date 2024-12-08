namespace Zapto.Component.Common.Models
{
    public sealed record LocationModel : BaseModel
	{
        public string? Location { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public byte LocalizationIsAvailable { get; set; } = ProgressStatus.None;
        public byte LocationIsAvailable { get; set; } = ProgressStatus.None;

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Zip))
            {
                return $"{Location} - {State} - {Country} -  {Latitude!.Value:F3} - {Longitude!.Value:F3}";
            }
            else
            {
                return $"{Location} - {Zip} - {Country} -  {Latitude!.Value:F3} - {Longitude!.Value:F3}";
            }
        }
    }
}

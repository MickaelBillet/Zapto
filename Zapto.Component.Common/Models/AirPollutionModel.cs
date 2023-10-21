using Zapto.Component.Common.Helpers;

namespace Zapto.Component.Common.Models
{
    public class AirPollutionModel : BaseModel
    {
        public string? Location { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double? aqi { get; set; }
        public List<AirPollutionItemModel>? Items { get; set; } = new List<AirPollutionItemModel>();
        public DateTime? TimeStamp { get; set; }
        public string GetAQIColor()
        {
            string color = string.Empty;
            if (this.aqi == 1) { color = ZaptoColors.Green1; }
            else if (this.aqi == 2) { color = ZaptoColors.Orange2; }
            else if (this.aqi == 3) { color = ZaptoColors.Orange3; }
            else if (this.aqi == 4) { color = ZaptoColors.Orange4; }
            else if (this.aqi == 5) { color = ZaptoColors.Red5;  }
            return color;
        }
    }
}

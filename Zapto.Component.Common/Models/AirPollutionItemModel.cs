using Zapto.Component.Common.Helpers;

namespace Zapto.Component.Common.Models
{
    public class AirPollutionItemModel : BaseModel
    {
        public string? Name { get; set; }
        public double? Value { get; set; }
        public int[]? Levels { get; set; }

        public string GetLevelColor()
        {
            string color = string.Empty;
            if (this.Levels != null)
            {
                if (this.Value >= this.Levels[0] && this.Value < this.Levels[1]) { color = ZaptoColors.Green1; }
                else if (this.Value >= this.Levels[1] && this.Value < this.Levels[2]) { color = ZaptoColors.Orange2; }
                else if (this.Value >= this.Levels[2] && this.Value < this.Levels[3]) { color = ZaptoColors.Orange3; }
                else if (this.Value >= this.Levels[3] && this.Value < this.Levels[4]) { color = ZaptoColors.Orange4; }
                else if (this.Value >= this.Levels[4]) { color = ZaptoColors.Red5; }
            }
            return color;
        }
    }
}

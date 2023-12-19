using Zapto.Component.Common.Helpers;

namespace Zapto.Component.Common.Models
{
    public class AirPollutionItemModel : BaseModel
    {
        #region Properties
        public string? Description { get; set; }
        public string? Name { get; set; }
        public double? Value { get; set; }
        public int[]? Levels { get; set; }
        public bool? HasStatus { get; set; }
        #endregion

        #region Constructor
        public AirPollutionItemModel(string? description, string? name, double? value, int[]? levels) 
        {
            this.Name = name;
            this.Value = value;
            this.Levels = levels;
            this.HasStatus = (levels?.Any() == true);
            this.Description = description;
        }
        #endregion

        #region Methods
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

        public string GetLevelColor(int level)
        {
            string color = string.Empty;
            if (level == 0) { color = ZaptoColors.Green1; }
            else if (level == 1) { color = ZaptoColors.Orange2; }
            else if (level == 2) { color = ZaptoColors.Orange3; }
            else if (level == 3) { color = ZaptoColors.Orange4; }
            else if (level == 4) { color = ZaptoColors.Red5; }
            return color;
        }
        #endregion
    }
}

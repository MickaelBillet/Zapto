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
        public List<(string description, string color)>? LevelDescriptions { get; set; }
        #endregion

        #region Constructor
        public AirPollutionItemModel(string? name, string? description, double? value, int[]? levels) 
        {
            this.Name = name;
            this.Value = value;
            this.Levels = levels;
            this.Description = description;

            if (this.Levels != null)
            {
                this.LevelDescriptions = new(this.Levels.Length)
                {
                    ($"[{this.Levels[0]},{this.Levels[1]})", ZaptoColors.Green1),
                    ($"[{this.Levels[1]},{this.Levels[2]})", ZaptoColors.Orange2),
                    ($"[{this.Levels[2]},{this.Levels[3]})", ZaptoColors.Orange3),
                    ($"[{this.Levels[3]},{this.Levels[4]})", ZaptoColors.Orange4),
                    ($">={this.Levels[4]}", ZaptoColors.Red5)
                };
            }
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
        #endregion
    }
}

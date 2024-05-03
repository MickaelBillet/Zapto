using Zapto.Component.Common.Helpers;

namespace Zapto.Component.Common.Models
{
    public record HealthcheckItemModel : BaseModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public string? Exception { get; set; }
        public string? Tags { get; set; }
        public string? Data { get; set; }

        public string GetStatusColor()
        {
            string color = string.Empty;
            if (this.Status == 0) { color = ZaptoColors.Red5; }
            else if (this.Status == 1) { color = ZaptoColors.Orange3; }
            else if (this.Status == 2) { color = ZaptoColors.Green1; }
            return color;
        }
    }
}

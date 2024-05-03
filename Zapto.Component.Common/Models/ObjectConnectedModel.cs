namespace Zapto.Component.Common.Models
{
    public abstract record ObjectConnectedModel : BaseModel
	{		
		public string? Name { get; set; }
		public string LocationId { get; set; } = string.Empty;
	}
}

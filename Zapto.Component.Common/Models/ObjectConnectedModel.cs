namespace Zapto.Component.Common.Models
{
    public abstract class ObjectConnectedModel : BaseModel
	{		
		public string? Name { get; set; }
		public string LocationId { get; set; } = string.Empty;
	}
}

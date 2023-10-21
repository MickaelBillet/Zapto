namespace Framework.Core.Domain
{
	public class Logs : Item
    {
		public string Level { get; set; }
		public string Exception { get; set; }
		public string RenderedMessage { get; set; }
		public string Properties { get; set; }
	}
}

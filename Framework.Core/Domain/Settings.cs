namespace Framework.Core.Domain
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public class Settings
    {
        public string BackEndUrl { get; set; }
		public string PortHttp { get; set; }
		public string PortHttps { get; set; }

	}
}
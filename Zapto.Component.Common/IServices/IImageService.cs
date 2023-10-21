namespace Zapto.Component.Common.Services
{
	public interface IImageService
	{
		Task<Stream?> GetImageStreamAsync(string url);
	}
}

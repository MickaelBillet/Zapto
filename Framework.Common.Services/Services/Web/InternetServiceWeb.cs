namespace Framework.Infrastructure.Services
{
    public class InternetServiceWeb : InternetService
	{
		public override bool IsConnectedToInternet()
		{
			return true;
		}
	}
}

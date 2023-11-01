using Framework.Infrastructure.Services;
using Xamarin.Essentials;

namespace Framework.Mobile.Services
{
    public class InternetServiceMobile : InternetService
	{
		public override bool IsConnectedToInternet()
		{
			return ((int)Connectivity.NetworkAccess > 1) ? true : false;
		}
	}
}

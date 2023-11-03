using Android.Content;
using Android.Content.PM;
using Framework.Infrastructure.Services;

[assembly: Xamarin.Forms.Dependency(typeof(AirZapto.Droid.Services.Version))]

namespace AirZapto.Droid.Services
{
    public class Version : IVersion
    {
        public string GetVersionNumber()
        {
            Context context = Android.App.Application.Context;
            PackageManager manager = context.PackageManager;
            PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);
            return info.VersionName;
        }
    }
}


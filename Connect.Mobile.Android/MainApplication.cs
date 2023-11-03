using Android.App;
using Android.Runtime;
using Connect.Mobile.Droid.Services;
using Connect.Mobile.Interfaces;
using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Connect.Mobile.Droid
{
    [Application]
    public class MainApplication : Android.App.Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            Host.Current.Initialize(this.ConfigureServices);
            Host.Current.GetRequiredService<IFirebaseMobileService>().Initialize(this);
            Host.Current.GetRequiredService<IFirebaseMobileService>().ReceiveNotification();
        }

        private void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            services.AddSingleton<ILocalize, Localize>();
            services.AddSingleton<IFileHelper, FileHelper>();
            services.AddSingleton<INotification, NotificationHelper>();
            services.AddSingleton<IForegroundService, ForegroundServiceAndroid>();
            services.AddSingleton<IFirebaseMobileService, FirebaseMobileService>();
        }
    }
}
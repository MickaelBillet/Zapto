using Connect.Model;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Connect.Application.Infrastructure
{
    public static class AlertServiceFactory
    {
        public static IAlertService? CreateAlerteService(IServiceProvider serviceProvider,  short type)
        {
            IAlertService? alertService = null;

            if (type == ServiceAlertType.Firebase)
            {
                alertService = serviceProvider.GetService<IFirebaseService>();
            }
            else if (type == ServiceAlertType.SignalR)
            {
                alertService = serviceProvider.GetService<ISignalRConnectService>();
            }

            return alertService;
        }
    }
}

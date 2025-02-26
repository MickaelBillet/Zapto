using Framework.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Connect.WebServer.Helpers
{
    public static class SerialCommunicationHelper
    {
        public static void StartSerialCommunicationReception(this IServiceProvider services)
        {
            using (IServiceScope scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                ISerialCommunicationService? service = scope.ServiceProvider.GetService<ISerialCommunicationService>();
                service?.Receive();
            }
        }
    }
}

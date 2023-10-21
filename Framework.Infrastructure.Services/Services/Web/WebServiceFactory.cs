using System;

namespace Framework.Infrastructure.Services
{
    public static class WebServiceFactory
    {
        public static IWebService CreateWebService(IServiceProvider serviceProvider, string httpClientName)
        {
            return new WebService(serviceProvider, httpClientName);
        }
    }
}

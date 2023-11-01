using Framework.Core.Base;
using System;
using System.Net.Http;

namespace Framework.Mobile.Core.Services
{
	public class CustumHttpMessageHandler : HttpClientHandler
    {
        public CustumHttpMessageHandler(string urlToAuthorize, string url) : base()
        {
			ClientCertificateOptions = ClientCertificateOption.Manual;
			ServerCertificateCustomValidationCallback = (httpRequestMessage, certificate, cetChain, policyErrors) =>
			{
				if (urlToAuthorize.Contains(url))
				{
					if ((DateTime.Parse(certificate.GetExpirationDateString()) >= Clock.Now) && (certificate.Subject.Contains(url)))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return true;
				}
			};
		}
    }
}

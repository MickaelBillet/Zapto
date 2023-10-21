using Framework.Core.Base;
using System;

namespace Framework.Infrastructure.Services
{
    public interface IInternetService : IDisposable
	{
		public string GetMacAddress();
		public bool IsConnectedToInternet();
		public event EventHandler<ConnectionArgs> ConnectivityChanged;
	}
}

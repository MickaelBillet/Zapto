using Framework.Core.Base;
using System;
using System.Net.NetworkInformation;
using System.Timers;
using Timer = System.Timers.Timer;

#nullable disable

namespace Framework.Infrastructure.Services
{
    public abstract class InternetService : IInternetService
	{
		#region Properties

		private Timer Timer { get; set; }

		private bool IsConnected { get; set; } = false;

        public event EventHandler<ConnectionArgs> ConnectivityChanged;

        #endregion

        #region Constructor

        public InternetService()
		{
			this.Timer = new Timer(5000);
			this.Timer.Elapsed += TimerElapsed;
			this.Timer.Start();
		}

		#endregion

		#region Methods

		public void Dispose()
		{
			if (this.Timer != null)
			{
				this.Timer.Stop();
				this.Timer.Elapsed -= TimerElapsed;
			}
		}

		private void TimerElapsed(object sender, ElapsedEventArgs e)
		{
			bool isConnected = this.IsConnectedToInternet();

			if (isConnected != this.IsConnected)
			{
				this.IsConnected = isConnected;
				this.ConnectivityChanged?.Invoke(sender, new ConnectionArgs() { IsConnected = isConnected });
			}
		}

		public string GetMacAddress()
		{
			string macAddresses = string.Empty;

			foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
			{
				if (nic.OperationalStatus == OperationalStatus.Up)
				{
					macAddresses += nic.GetPhysicalAddress().ToString();
					break;
				}
			}

			return macAddresses;
		}

		public abstract bool IsConnectedToInternet();

		#endregion
	}
}

using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Zapto.Component.Common.Services;

namespace Zapto.Component.Common.ViewModels
{
    public interface IBaseViewModel : IDisposable
	{
		public Task InitializeAsync(string? parameter);
        public event EventHandler Refresh;
	}

	public class BaseViewModel : IBaseViewModel
	{
		private bool _disposed = false;

		public event EventHandler? Refresh;

		#region Properties
		public INavigationService NavigationService { get; }
        protected IAuthenticationService AuthenticationService { get; }
        #endregion

        #region Constructor
        public BaseViewModel(IServiceProvider serviceProvider)
		{
			this.NavigationService = serviceProvider.GetRequiredService<INavigationService>();
			this.AuthenticationService = serviceProvider.GetRequiredService<IAuthenticationService>();
		}
		#endregion

		#region Methods

		protected virtual void OnRefresh(EventArgs e)
		{
			EventHandler? handler = this.Refresh;
			handler?.Invoke(this, e);
		}

		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}        

		public virtual async Task InitializeAsync(string? parameter)
		{
			await Task.FromResult<bool>(true);
		}

		// Protected implementation of Dispose pattern.
		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				if (_disposed)
					return;

				if (disposing)
				{

				}

				_disposed = true;
			}

			_disposed = true;
		}
		#endregion
	}
}

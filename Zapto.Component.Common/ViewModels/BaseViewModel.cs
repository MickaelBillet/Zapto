using Framework.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Zapto.Component.Common.Resources;
using Zapto.Component.Common.Services;

namespace Zapto.Component.Common.ViewModels
{
    public interface IBaseViewModel : IDisposable
	{
		public Task InitializeAsync(object? parameter);
        public event EventHandler Refresh;
		public bool IsLoading { get; }
	}

	public class BaseViewModel : IBaseViewModel
	{
		private bool _disposed = false;

        public event EventHandler? Refresh;

        #region Properties
        protected INavigationService NavigationService { get; }
        protected IAuthenticationService AuthenticationService { get; }
        protected IStringLocalizer<Resource> Localizer { get; }
        public bool IsLoading { get; protected set; } = false;
        #endregion

        #region Constructor
        public BaseViewModel(IServiceProvider serviceProvider)
		{
			this.NavigationService = serviceProvider.GetRequiredService<INavigationService>();
			this.AuthenticationService = serviceProvider.GetRequiredService<IAuthenticationService>();
            this.Localizer = serviceProvider.GetRequiredService<IStringLocalizer<Resource>>();
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

		public virtual async Task InitializeAsync(object? parameter)
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

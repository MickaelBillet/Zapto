using System;
using System.Threading.Tasks;
using Connect.Mobile.ViewModel;

namespace Connect.Mobile.Interfaces
{
	public interface INavigationService
	{
		Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel;

        Task NavigateToAsync<TViewModel>(Object parameter) where TViewModel : BaseViewModel;

        Task NavigateToModalAsync<TViewModel>(Object parameter, Func<Object, Task> validateCallback, Func<Object, Task> cancelCallback) where TViewModel : BaseViewModel;

		Task RemovePageFromStackAsync<TViewModel>() where TViewModel : BaseViewModel;

		Task RemoveLastFromStackAsync();

        Task RemoveModalAsync();

		Task SetRootPage<TViewModel>(Object parameter);

		BaseViewModel CurrentPageViewModel { get; }
	}
}

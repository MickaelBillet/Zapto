using System;
using System.Threading.Tasks;
using AirZapto.ViewModel;

namespace AirZapto.Mobile.Interfaces
{
	public interface INavigationService
	{
		Task PopupNavigateToAsync<TViewModel>(Object parameter) where TViewModel : BaseViewModel;
		Task RemovePopupAsync();
		Task NavigateToAsync<TViewModel>(Object parameter) where TViewModel : BaseViewModel;
		Task NavigateToModalAsync<TViewModel>(Object parameter, Func<Object, Task> validateCallback, Func<Object, Task> cancelCallback) where TViewModel : BaseViewModel;
		Task RemoveLastFromStackAsync();
		Task RemoveModalAsync();
	}
}

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using AirZapto.Mobile.Interfaces;
using AirZapto.ViewModel;
using AirZapto.View.Controls;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Pages;

namespace AirZapto.Services
{
	public class NavigationService : INavigationService
    {
        #region Property

        #endregion

        #region Public

        public async Task NavigateToAsync<TViewModel>(Object parameter) where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), parameter).ConfigureAwait(true);
        }

        public async Task NavigateToModalAsync<TViewModel>(Object parameter, Func<Object, Task> validateCallback, Func<Object, Task> cancelCallback) where TViewModel : BaseViewModel
        {
            await InternalNavigateToModalAsync(typeof(TViewModel), parameter, validateCallback, cancelCallback).ConfigureAwait(true);
        }

        public async Task RemoveLastFromStackAsync()
        {            
            await Shell.Current.Navigation.PopAsync(true).ConfigureAwait(true);
        }

        public async Task RemoveModalAsync()
        {
            await Shell.Current.Navigation.PopModalAsync().ConfigureAwait(true);
        }

        public async Task PopupNavigateToAsync<TViewModel>(Object parameter) where TViewModel : BaseViewModel
        {
            await InternalNavigatePopupToAsync(typeof(TViewModel), parameter).ConfigureAwait(true);
        }

        public async Task RemovePopupAsync()
		{
            await PopupNavigation.Instance.PopAsync().ConfigureAwait(true);
		}

        #endregion

        #region Private

        private async Task InternalNavigateToModalAsync(Type viewModelType, Object parameter, Func<Object, Task> validateCallback, Func<Object, Task> cancelCallback)
        {
            Page page = CreatePage(viewModelType, parameter);

            await Shell.Current.PushModalPageAsync(page).ConfigureAwait(true);

            await (page.BindingContext as BaseViewModel).Initialize(parameter, validateCallback, cancelCallback).ConfigureAwait(true);
        }

        private async Task InternalNavigateToAsync(Type viewModelType, Object parameter)
        {
            try
            {
                Page page = CreatePage(viewModelType, parameter);

                await Shell.Current.PushPageAsync(page, true).ConfigureAwait(true);

                await (page.BindingContext as BaseViewModel).Initialize(parameter).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async Task InternalNavigatePopupToAsync(Type viewModelType, Object parameter)
        {
            try
            {
                PopupPage page = CreatePopupPage(viewModelType, parameter);

                await PopupNavigation.Instance.PushAsync(page).ConfigureAwait(true);

                await (page.BindingContext as BaseViewModel).Initialize(parameter).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private Type GetPageTypeForViewModel(Type viewModelType, String idiom)
        {
            string viewName = viewModelType.FullName.Replace("Model", idiom);
            string viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
			string viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);

			Type  viewType = Type.GetType(viewAssemblyName);

            //We test with null because we don't want call infinitely this method
            if ((viewType == null) && (!string.IsNullOrEmpty(idiom)))
            {
                viewType = this.GetPageTypeForViewModel(viewModelType, String.Empty);
            }

			return viewType;
        }

        private Page CreatePage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType, Device.Idiom.ToString());

            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            return page;
        }

        private PopupPage CreatePopupPage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType, Device.Idiom.ToString());

            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            PopupPage page = Activator.CreateInstance(pageType) as PopupPage;
            return page;
        }
    }

    #endregion
}

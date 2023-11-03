using Connect.Mobile.Interfaces;
using Connect.Mobile.View;
using Connect.Mobile.View.Controls;
using Connect.Mobile.ViewModel;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Connect.Mobile.Services
{
    public class NavigationService : INavigationService
    {
        #region Property

        private MainView MainPage { get; set; }

        #endregion

        #region Public

        public BaseViewModel CurrentPageViewModel
        {
            get
            {
                CustomNavigationView mainPage = Xamarin.Forms.Application.Current.MainPage as CustomNavigationView;
                object viewModel = mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 1].BindingContext;
                return viewModel as BaseViewModel;
            }
        }

        public async Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public async Task NavigateToAsync<TViewModel>(Object parameter) where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public async Task NavigateToModalAsync<TViewModel>(Object parameter, Func<Object, Task> validateCallback, Func<Object, Task> cancelCallback) where TViewModel : BaseViewModel
        {
            await InternalNavigateToModalAsync(typeof(TViewModel), parameter, validateCallback, cancelCallback);
        }

        public async Task RemovePageFromStackAsync<TViewModel>() where TViewModel : BaseViewModel
        {
            await this.InternalRemovePageAsync(typeof(TViewModel), (Page p) => p.BindingContext is TViewModel);
        }

        public async Task RemoveLastFromStackAsync()
        {
            CustomNavigationView mainPage = Xamarin.Forms.Application.Current.MainPage as CustomNavigationView;

            if (mainPage != null)
            {
                await mainPage.PopAsync(true);
			}
        }

        public async Task RemoveModalAsync()
        {
            Page mainPage = Xamarin.Forms.Application.Current.MainPage as Page;

            if (mainPage != null)
            {
                await mainPage.Navigation.PopModalAsync(false);
            }
        }

        public async Task SetRootPage<TViewModel>(object parameter)
        {
            Page page = CreatePage(typeof(TViewModel), parameter);

            if (page != null)
            {
                await this.MainPage.Detail.Navigation.PushAsync(page, true);

                this.MainPage.IsPresented = false;

                for (int i = 0; i < this.MainPage.Detail.Navigation.NavigationStack.Count - 1;)
                {
                    this.MainPage.Detail.Navigation.RemovePage(this.MainPage.Detail.Navigation.NavigationStack[i]);
                }

                (page.BindingContext as BaseViewModel).Initialize(parameter);
            }
        }

        #endregion

        #region Private

        private async Task InternalRemovePageAsync(Type viewModelType, Func<Page, bool> func)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType, Device.Idiom.ToString());

            if (pageType != null)
            {
                CustomNavigationView mainPage = Xamarin.Forms.Application.Current.MainPage as CustomNavigationView;

                if (mainPage != null)
                {
                    await Task.Run(() =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                           Page page = mainPage.Navigation.NavigationStack.FirstOrDefault(func);

                           if (page != null)
                           {
                               mainPage.Navigation.RemovePage(page);
                           }
                        });
                    });
                }
            }
        }

        private async Task InternalNavigateToModalAsync(Type viewModelType, Object parameter, Func<Object, Task> validateCallback, Func<Object, Task> cancelCallback)
        {
            Page page = CreatePage(viewModelType, parameter);

			if (page.GetType().Name.Contains("LoginView"))
			{
                (page.BindingContext as BaseViewModel).Initialize(parameter, validateCallback, cancelCallback);

				Xamarin.Forms.Application.Current.MainPage = new CustomNavigationView(page);
			}
            else
            {
                await Task.Run(() => (page.BindingContext as BaseViewModel).Initialize(parameter, validateCallback, cancelCallback)).ContinueWith((t) =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        CustomNavigationView navigationPage = Xamarin.Forms.Application.Current.MainPage as CustomNavigationView;

                        if (navigationPage != null)
                        {
                            await navigationPage.PushModalPageAsync(page);
                        }
                        else
                        {
                            await Xamarin.Forms.Application.Current.MainPage.PushModalPageAsync(new CustomNavigationView(page));
                        }
                    });
                });
            }
        }

        private async Task InternalNavigateToAsync(Type viewModelType, Object parameter)
        {
            try
            {
                Page page = CreatePage(viewModelType, parameter);

                if (page.GetType().Name.Contains("MainView"))
                {
                    Xamarin.Forms.Application.Current.MainPage = page;

                    this.MainPage = page as MainView;

                    (page.BindingContext as BaseViewModel).Initialize(parameter);
                }
                else
                {
                    if (Xamarin.Forms.Application.Current.MainPage is CustomNavigationView)
                    {
                        CustomNavigationView navigationPage = Xamarin.Forms.Application.Current.MainPage as CustomNavigationView;

                        await navigationPage.PushPageAsync(page, true);
                    }
                    else if (Xamarin.Forms.Application.Current.MainPage is MainView)
                    {
                        await this.MainPage.Detail.PushPageAsync(page, true);

                        this.MainPage.IsPresented = false;
                    }

                    (page.BindingContext as BaseViewModel).Initialize(parameter);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
        }

        private Type GetPageTypeForViewModel(Type viewModelType, String idiom)
        {
            string viewName = viewModelType.FullName.Replace("Model", idiom);
            string viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
			string viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);

			Type viewType = Type.GetType(viewAssemblyName);

            //We test with null because we don't want call infinitely this method
            if ((viewType == null) && (!String.IsNullOrEmpty(idiom)))
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
    }

    #endregion
}

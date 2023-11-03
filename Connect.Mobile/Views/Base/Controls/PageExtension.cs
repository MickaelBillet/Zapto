using System.Threading.Tasks;
using Xamarin.Forms;

namespace Connect.Mobile.View.Controls
{
    public static class PageExtension
    {
        public static async Task PushModalPageAsync(this Page mainPage, Page modalPage)
        {
            if (modalPage != null)
            {
                if (mainPage.Navigation.ModalStack.Count > 0)
                {
                    if ((mainPage.Navigation.ModalStack[^1] is CustomNavigationView) && (modalPage is CustomNavigationView))
                    {
                        if ((mainPage.Navigation.ModalStack[^1] as CustomNavigationView).CurrentPage.GetType() == (modalPage as CustomNavigationView).CurrentPage.GetType())
                        {
                            await mainPage.Navigation.PopModalAsync();
                        }
                    }
                }

                await mainPage.Navigation.PushModalAsync(modalPage, false);
            }
        }

        public static async Task PushPageAsync(this Page mainPage, Page page, bool animated)
        {
            if (page != null)
            {
                if (mainPage.Navigation.NavigationStack.Count > 0)
                {
                    if ((mainPage.Navigation.NavigationStack[^1] is DetailView) && (page is DetailView))
                    {
                        if ((mainPage.Navigation.NavigationStack[^1] as DetailView).GetType() == (page as DetailView).GetType())
                        {
                            await mainPage.Navigation.PopAsync();
                        }
                    }
                }
              
                await mainPage.Navigation.PushAsync(page, animated);
            }
        }
    }
}

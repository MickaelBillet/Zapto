using System;
using System.Threading.Tasks;

namespace Connect.Mobile.ViewModel
{
    public interface IPageLifeCylceEvents
    {
        Task OnAppearing();
        Task OnDisappearing();
    }
}

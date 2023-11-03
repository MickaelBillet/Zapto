using System;
using System.Threading.Tasks;

namespace AirZapto.ViewModel
{
    public interface IPageLifeCylceEvents
    {
        Task OnAppearing();
        Task OnDisappearing();
    }
}

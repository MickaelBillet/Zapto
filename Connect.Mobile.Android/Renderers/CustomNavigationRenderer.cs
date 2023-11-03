using System.Threading.Tasks;
using Android.Content;
using Connect.Mobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNavigationRenderer))]

namespace Connect.Mobile.Droid.Renderers
{
    public class CustomNavigationRenderer : NavigationPageRenderer
    {
        public CustomNavigationRenderer(Context context) : base(context)
        {
        }

        protected override async Task<bool> OnPopViewAsync(Page page, bool animated)
        {
            return await  base.OnPopViewAsync(page, animated);
        }
    }
}
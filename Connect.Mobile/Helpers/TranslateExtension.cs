using Framework.Infrastructure.Services;
using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Connect.Mobile.Helpers
{
    // You exclude the 'Extension' suffix when using in Xaml markup
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        #region Property

        private CultureInfo CultureInfo { get; set; }

        public string Text { get; set; }

        private const String ResourceId = "Connect.Mobile.Resources.AppResources";

        #endregion

        #region Constructor

        public TranslateExtension()
        {
            this.CultureInfo = App.CurrentCulture;

            if ((Device.RuntimePlatform == Device.iOS) || (Device.RuntimePlatform == Device.Android))
            {
                CultureInfo = Host.Current.GetService<ILocalize>().GetCurrentCultureInfo();
            }
        }

        #endregion

        #region Method
         
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            ResourceManager manager = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);

            string translation = manager.GetString(Text, CultureInfo);

            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, App.ResourcesFiles, CultureInfo.Name), "Text");
#else
            translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }

            return translation;
        }

        #endregion
    }
}

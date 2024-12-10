using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Zapto.Component.Common.Services;

namespace Zapto.Component.Common.ViewModels
{
    public interface IChooseLanguageViewModel : IDisposable, IBaseViewModel
    {
        public CultureInfo SelectedCulture { get; set; }
        public CultureInfo[]? Cultures { get; }
    }

	public sealed class ChooseLanguageViewModel : BaseViewModel, IChooseLanguageViewModel
    {
		#region Properties
        private IZaptoLocalStorageService StorageService { get; set; }

        public CultureInfo[]? Cultures { get; private set; }

        public CultureInfo SelectedCulture
        {
            get => CultureInfo.CurrentCulture;
            set
            {
                if (CultureInfo.CurrentCulture != value)
                {
                    this.StorageService.SetItemAsync<string>("culture", value.Name);

#if DEBUG
                    // Load the Current URL
                    this.NavigationService.NavigateTo("http://localhost:5207/", true);
#else
                    this.NavigationService.NavigateTo("https://dashboard.connect-zapto.fr/", true);
#endif
                }
            }
        }

        #endregion

        #region Constructor
        public ChooseLanguageViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.StorageService = serviceProvider.GetRequiredService<IZaptoLocalStorageService>();
        }
		#endregion

		#region Methods
		public override async Task InitializeAsync(object? parameter)
        {
            await Task.Run(() =>
            {
                this.Cultures = new[] { new CultureInfo("en-US"), new CultureInfo("fr-FR") };
            });
        }
		#endregion
	}
}

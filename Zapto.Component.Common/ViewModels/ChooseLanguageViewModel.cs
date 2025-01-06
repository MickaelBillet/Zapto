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
        private IZaptoStorageService StorageService { get; set; }

        public CultureInfo[]? Cultures { get; private set; }

        public CultureInfo SelectedCulture
        {
            get => CultureInfo.CurrentCulture;
            set
            {
                if (CultureInfo.CurrentCulture != value)
                {
                    Task.Run(async () =>
                    {
                        const string defaultCulture = "en-US";
                        await this.StorageService.SetItemAsync<string>("blazorCulture", value.Name);
                        var result = await this.StorageService.GetItemAsync<string>("blazorCulture");
                        var culture = CultureInfo.GetCultureInfo(result ?? defaultCulture);
                        if (result == null)
                        {
                            await this.StorageService.SetItemAsync<string>("blazorCulture", defaultCulture);
                        }

                        CultureInfo.DefaultThreadCurrentCulture = culture;
                        CultureInfo.DefaultThreadCurrentUICulture = culture;
#if DEBUG
                        // Load the Current URL
                        this.NavigationService.NavigateTo("http://localhost:5207/", true);
#else
                    this.NavigationService.NavigateTo("https://dashboard.connect-zapto.fr/", true);
#endif
                    });
                }
            }
        }

        #endregion

        #region Constructor
        public ChooseLanguageViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.StorageService = serviceProvider.GetRequiredService<IZaptoStorageService>();
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

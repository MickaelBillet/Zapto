using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

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
		private ILocalStorageService LocalStorageService { get; set; }

        public CultureInfo[]? Cultures { get; private set; }

        public CultureInfo SelectedCulture
        {
            get => CultureInfo.CurrentCulture;
            set
            {
                if (CultureInfo.CurrentCulture != value)
                {
                    this.LocalStorageService.SetItemAsync<string>("culture", value.Name);
                    this.NavigationService.NavigateTo(this.NavigationService.GetUri(), true);
                }
            }
        }

        #endregion

        #region Constructor
        public ChooseLanguageViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.LocalStorageService = serviceProvider.GetRequiredService<ILocalStorageService>();
        }
		#endregion

		#region Methods
		public override async Task InitializeAsync(string? parameter)
        {
            await Task.Run(() =>
            {
                this.Cultures = new[] { new CultureInfo("en-US"), new CultureInfo("fr-FR") };
            });
        }
		#endregion
	}
}

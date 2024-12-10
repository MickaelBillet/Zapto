using Microsoft.Extensions.DependencyInjection;
using Zapto.Component.Common.Services;

namespace Zapto.Component.Common.ViewModels
{
    public interface ISelectDateViewModel : IBaseViewModel
    {
    }

    public class SelectDateViewModel : BaseViewModel, ISelectDateViewModel
    {
        #region Properties
        private IZaptoStorageService ZaptoLocalStorageService { get; }
        #endregion

        #region Constructor
        public SelectDateViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.ZaptoLocalStorageService = serviceProvider.GetRequiredService<IZaptoStorageService>();
        }
        #endregion
    }
}

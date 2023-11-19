namespace Zapto.Component.Common.ViewModels
{
    public interface IHomeViewModel : IBaseViewModel
    {
    }

    public sealed class HomeViewModel : BaseViewModel, IHomeViewModel
    {
        #region Constructor
        public HomeViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        #endregion

        #region Methods
        #endregion
    }
}

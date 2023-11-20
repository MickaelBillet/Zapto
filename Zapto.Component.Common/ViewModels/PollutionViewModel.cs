namespace Zapto.Component.Common.ViewModels
{
    public interface IPollutionViewModel : IBaseViewModel
    {

    }


    public sealed class PollutionViewModel : BaseViewModel, IPollutionViewModel
    {
        #region Properties

        #endregion

        #region Constructor
        public PollutionViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        #endregion
    }
}

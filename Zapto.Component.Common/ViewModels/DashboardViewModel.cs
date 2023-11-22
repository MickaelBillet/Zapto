namespace Zapto.Component.Common.ViewModels
{
    public interface IDashboardViewModel : IBaseViewModel
	{
    }

    public sealed class DashboardViewModel : BaseViewModel, IDashboardViewModel
	{
        #region Properties
        #endregion

        #region Constructor
        public DashboardViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
        }
        #endregion
    }
}

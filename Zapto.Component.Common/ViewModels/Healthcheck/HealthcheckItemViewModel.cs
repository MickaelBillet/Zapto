namespace Zapto.Component.Common.ViewModels
{
    public interface IHealthCheckItemViewModel : IBaseViewModel
    {
    }

    public class HealthCheckItemViewModel : BaseViewModel, IHealthCheckItemViewModel
    {
        #region Properties

        #endregion

        #region Constructor
        public HealthCheckItemViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        #endregion

        #region Methods

        #endregion
    }
}

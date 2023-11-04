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

        #region Methods

        public override async Task InitializeAsync(string? parameter)
        {
            await base.InitializeAsync(parameter);
        }
        public override void Dispose()
        {
            base.Dispose();
        }

        #endregion
    }
}

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

		#region Methods
		public override async Task InitializeAsync(string? parameter)
		{
			await base.InitializeAsync(parameter);
		}
        #endregion
    }
}

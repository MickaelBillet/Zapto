namespace Zapto.Component.Common.ViewModels
{
    public interface IMainViewModel : IBaseViewModel
	{

	}

	public sealed class MainViewModel : BaseViewModel, IMainViewModel
	{
		#region Constructor
		public MainViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
		}
        #endregion
    }
}

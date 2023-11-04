using Microsoft.Extensions.Configuration;

namespace Zapto.Component.Common.ViewModels
{
    public interface IMainViewModel : IBaseViewModel
	{

	}

	public sealed class MainViewModel : BaseViewModel, IMainViewModel
	{
		#region Properties
		#endregion

		#region Constructor
		public MainViewModel(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider)
		{
		}
		#endregion

		#region Methods

		#endregion
	}
}

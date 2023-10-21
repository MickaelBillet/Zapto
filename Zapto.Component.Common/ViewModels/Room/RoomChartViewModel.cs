using System.Diagnostics;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface IRoomChartViewModel : IBaseViewModel
    {

    }

    internal class RoomChartViewModel : BaseViewModel, IRoomChartViewModel
    {
        #region Properties
        #endregion

        #region Constructor
        public RoomChartViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
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

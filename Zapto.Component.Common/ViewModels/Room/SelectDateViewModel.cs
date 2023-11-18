using Connect.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Zapto.Component.Common.ViewModels
{
    public interface ISelectDateViewModel : IBaseViewModel
    {
        Task<DateTime?> GetRoomMaxDate(string roomId);
        Task<DateTime?> GetRoomMinDate(string roomId);
    }

    public class SelectDateViewModel : BaseViewModel, ISelectDateViewModel
    {
        #region Properties
        private IApplicationOperationDataService ApplicationOperationDataService { get; }
        #endregion

        #region Constructor
        public SelectDateViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.ApplicationOperationDataService = serviceProvider.GetRequiredService<IApplicationOperationDataService>();
        }
        #endregion

        #region Methods
        public async Task<DateTime?> GetRoomMaxDate(string roomId)
        {
            return (this.ApplicationOperationDataService != null) ? await this.ApplicationOperationDataService.GetRoomMaxDate(roomId) : null;
        }

        public async Task<DateTime?> GetRoomMinDate(string roomId)
        {
            return (this.ApplicationOperationDataService != null) ? await this.ApplicationOperationDataService.GetRoomMinDate(roomId) : null;
        }
        #endregion
    }
}

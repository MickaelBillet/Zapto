using Connect.Application;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

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
        public override async Task InitializeAsync(string? parameter)
        {
            await base.InitializeAsync(parameter);
        }

        public override void Dispose()
        {           
            base.Dispose();
        }

        public async Task<DateTime?> GetRoomMaxDate(string roomId)
        {
            try
            {
                return (this.ApplicationOperationDataService != null) ? await this.ApplicationOperationDataService.GetRoomMaxDate(roomId) : null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return null;
        }

        public async Task<DateTime?> GetRoomMinDate(string roomId)
        {
            try
            {
                return (this.ApplicationOperationDataService != null) ? await this.ApplicationOperationDataService.GetRoomMinDate(roomId) : null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return null;
        }
        #endregion
    }
}

using Connect.Application;
using Connect.Model;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface IRoomChartListViewModel : IBaseViewModel
    {
        Task<DateTime?> GetRoomMaxDate(string roomId);
        Task<DateTime?> GetRoomMinDate(string roomId);
        Task<IEnumerable<RoomChartModel>?> GetChartsData(DateTime? startDate, DateTime? endDate, string roomId);
    }

    public class RoomChartListViewModel : BaseViewModel, IRoomChartListViewModel
    {
        #region Properties
        private IApplicationOperationDataService ApplicationOperationDataService { get; }
        #endregion

        #region Constructor
        public RoomChartListViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.ApplicationOperationDataService = serviceProvider.GetRequiredService<IApplicationOperationDataService>();
        }
        #endregion

        #region Methods
        public override async Task InitializeAsync(string? parameter)
        {
            await base.InitializeAsync(parameter);
        }

        public async Task<DateTime?> GetRoomMaxDate(string roomId)
        {
            return (this.ApplicationOperationDataService != null) ? await this.ApplicationOperationDataService.GetRoomMaxDate(roomId) : null;
        }

        public async Task<DateTime?> GetRoomMinDate(string roomId)
        {
            return (this.ApplicationOperationDataService != null) ? await this.ApplicationOperationDataService.GetRoomMinDate(roomId) : null;
        }

        public async Task<IEnumerable<RoomChartModel>?> GetChartsData(DateTime? startDate, DateTime? endDate, string roomId)
        {
            List<RoomChartModel>? data = null;
            try
            {
                this.IsLoading = true;

                if (this.ApplicationOperationDataService != null)
                {
                    data = new List<RoomChartModel>();
                    for (DateTime? date = startDate; date <= endDate; date = date + new TimeSpan(1, 0, 0, 0))
                    {
                        IEnumerable<OperatingData?> operatingData = await this.ApplicationOperationDataService.GetRoomOperatingDataOfDay(roomId, date);
                        if ((operatingData != null) && operatingData.Any())
                        {
                            RoomChartModel model = new RoomChartModel()
                            {
                                Temperatures = operatingData.Select((data) => (decimal?)data?.Temperature).ToList(),
                                Labels = operatingData.Select((data) => $"{data?.Date.Hour}.{data?.Date.Minute}").ToList(),
                                Humidities = operatingData.Select((data) => (decimal?)data?.Humidity).ToList(),
                                Day = (date != null) ? date.Value.ToString("D") : string.Empty,
                            };                        
                            data.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex);
                throw new Exception("Charts Data Exception : " + ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
            return data;
        }
        #endregion
    }
}

using Connect.Application;
using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using Serilog;
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
            DateTime? dateTime = null;
            try
            {
                dateTime = (this.ApplicationOperationDataService != null) ? await this.ApplicationOperationDataService.GetRoomMaxDate(roomId) : null;
            }
            catch (Exception ex)
            {
                Log.Debug($"{ClassHelper.GetCallerClassAndMethodName()} - {ex.ToString()}");
                this.NavigationService.ShowMessage("GetRoomMaxDate Exception", ZaptoSeverity.Error);
            }
            return dateTime;
        }

        public async Task<DateTime?> GetRoomMinDate(string roomId)
        {
            DateTime? dateTime = null;
            try
            {
                dateTime = (this.ApplicationOperationDataService != null) ? await this.ApplicationOperationDataService.GetRoomMinDate(roomId) : null;
            }
            catch (Exception ex)
            {
                Log.Debug($"{ClassHelper.GetCallerClassAndMethodName()} - {ex.ToString()}");
                this.NavigationService.ShowMessage("GetRoomMinDate Exception", ZaptoSeverity.Error);
            }
            return dateTime;
        }

        public async Task<IEnumerable<RoomChartModel>?> GetChartsData(DateTime? startDate, DateTime? endDate, string roomId)
        {
            List<RoomChartModel>? models = null;
            try
            {
                this.IsLoading = true;

                if (this.ApplicationOperationDataService != null)
                {
                    models = new List<RoomChartModel>();
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
                            models.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                Log.Debug($"{ClassHelper.GetCallerClassAndMethodName()} - {ex.ToString()}");
                this.NavigationService.ShowMessage("Charts Data Exception", ZaptoSeverity.Error);
            }
            finally
            {
                this.IsLoading = false;
            }
            return models;
        }
        #endregion
    }
}

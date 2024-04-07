using Connect.Application;
using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WeatherZapto.Application.Services;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface IRoomChartViewModel : IBaseViewModel
    {
        Task<DateTime?> GetRoomMaxDate(string roomId);
        Task<DateTime?> GetRoomMinDate(string roomId);
        Task<IEnumerable<RoomChartModel>?> GetChartsData(DateTime? startDate, DateTime? endDate, string roomId);
    }

    public class RoomChartViewModel : BaseViewModel, IRoomChartViewModel
    {
        #region Properties
        private IApplicationOperationDataService ApplicationOperationDataService { get; }
        private IApplicationTemperatureService ApplicationTemperatureService { get; }
        private string? Location { get; set; }
        #endregion

        #region Constructor
        public RoomChartViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.ApplicationOperationDataService = serviceProvider.GetRequiredService<IApplicationOperationDataService>();
            this.ApplicationTemperatureService = serviceProvider.GetRequiredService<IApplicationTemperatureService>();
        }
        #endregion

        #region Methods
        public override async Task InitializeAsync(string? parameter)
        {
            await base.InitializeAsync(parameter);

            this.Location = parameter as string;
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
                        RoomChartModel model = new RoomChartModel();

                        IEnumerable<OperatingData?> operatingData = await this.ApplicationOperationDataService.GetRoomOperatingDataOfDay(roomId, date);
                        if ((operatingData != null) && operatingData.Any())
                        {
                            model.TemperaturesIN = operatingData.Select((data) => (decimal?)data?.Temperature).ToList();
                            model.Labels = operatingData.Select((data) => $"{data?.Date.Hour}.{data?.Date.Minute}").ToList();
                            model.Humidities = operatingData.Select((data) => (decimal?)data?.Humidity).ToList();
                            model.Day = (date != null) ? date.Value.ToString("D") : string.Empty;
                        };                        

                        IEnumerable<double?> temperaturesOut = await this.ApplicationTemperatureService.GetTemperatureOfDay(this.Location, date);
                        if ((temperaturesOut != null) && temperaturesOut.Any())
                        {
                            model.TemperaturesOUT = temperaturesOut.Select((data) => (decimal?)data).ToList();
                        }
                        
                        models.Add(model);                        
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

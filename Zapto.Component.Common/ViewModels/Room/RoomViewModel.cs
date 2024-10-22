using Connect.Application.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface IRoomViewModel : IBaseViewModel
	{
		public Task<bool> ReceiveStatusAsync(RoomModel model);
		public void OpenChart(RoomModel model);
    }

    public class RoomViewModel : BaseViewModel, IRoomViewModel
	{
		#region Properties
		private ISignalRService SignalRService { get; }
        #endregion

        #region Constructor
        public RoomViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.SignalRService = serviceProvider.GetRequiredService<ISignalRService>();
		}
		#endregion

		#region Methods
		public override async Task InitializeAsync(object? parameter)
		{
			await base.InitializeAsync(parameter);
		}

		public void OpenChart(RoomModel model)
		{
			if ((string.IsNullOrEmpty(model.Id) == false) 
				&& (string.IsNullOrEmpty(model.Name) == false) 
				&& (string.IsNullOrEmpty(model.LocationName) == false))
			{
				this.NavigationService.NavigateTo($"/roomchartlist/{model.LocationName}/{model.Id}/{model.Name}");
			}
		}

		public async Task<bool> ReceiveStatusAsync(RoomModel model)
		{
            bool res = false;
            try
            {
                res = await this.SignalRService.StartAsync(model.LocationId,
				null,
				async (roomStatus) =>
				{
					if (model?.Id == roomStatus.RoomId)
					{
						model.Humidity = (roomStatus.Humidity != null) ? roomStatus.Humidity.Value.ToString("00") : null;
						model.Temperature = (roomStatus.Temperature != null) ? roomStatus.Temperature.Value.ToString("00.0") : null;
						this.OnRefresh(new EventArgs());
						await Task.CompletedTask;
					}
				},
				null,
				null);
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                throw new Exception("SignalR Exception");
            }
			return res;
        }

		public override void Dispose()
		{
			this.SignalRService?.Dispose();
			base.Dispose();
		}
        #endregion
    }
}

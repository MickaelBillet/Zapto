using Connect.Application.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface IRoomViewModel : IBaseViewModel
	{
		public Task<bool> ReceiveStatusAsync(RoomModel model);
		public void OpenChart(string roomId, string roomName);
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

		public override async Task InitializeAsync(string? parameter)
		{
			await base.InitializeAsync(parameter);
		}

		public void OpenChart(string roomId, string roomName)
		{
			this.NavigationService.NavigateTo($"/roomchartlist/{roomId}/{roomName}");
		}

		public async Task<bool> ReceiveStatusAsync(RoomModel model)
		{
			try
			{
				return await this.SignalRService.StartAsync(model.LocationId,
				null,
				(roomStatus) =>
				{
					if (model?.Id == roomStatus.RoomId)
					{
						model.Humidity = (roomStatus.Humidity != null) ? roomStatus.Humidity.Value.ToString("00") : null;
						model.Temperature = (roomStatus.Temperature != null) ? roomStatus.Temperature.Value.ToString("00.0") : null;
						this.OnRefresh(new EventArgs());
					}
				},
				null,
				null);
			}
			catch (Exception ex) 
			{
				Debug.WriteLine(ex);
				throw ex;
			}
		}

		public override void Dispose()
		{
			this.SignalRService?.Dispose();
			base.Dispose();
		}
        #endregion
    }
}

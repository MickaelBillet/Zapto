using Connect.Application;
using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Diagnostics;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface IRoomListViewModel : IBaseViewModel
	{
		Task<IEnumerable<RoomModel>?> GetRoomModels(string? locationId);
	}

	public class RoomListViewModel : BaseViewModel, IRoomListViewModel
	{
		#region Properties
		private IApplicationRoomServices ApplicationRoomServices { get; }
		#endregion

		#region Constructor
		public RoomListViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.ApplicationRoomServices = serviceProvider.GetRequiredService<IApplicationRoomServices>();
		}
		#endregion

		#region Methods
		public async Task<IEnumerable<RoomModel>?> GetRoomModels(string? locationId)
		{
			IEnumerable<RoomModel>? models = null;
			try
			{
				this.IsLoading = true;

				models = (await this.ApplicationRoomServices.GetRoomsAsync(locationId))?.Select((room) => new RoomModel()
				{
					Name = this.Localizer[room.Name],
					LocationId = room.LocationId,
					Type = room.Type,
					Description = room.Description,
					Humidity = (room.Humidity != null) ? room.Humidity.Value.ToString("00") : null,
					Temperature = (room.Temperature != null) ? room.Temperature.Value.ToString("0.0") : null,
					DeviceType = room.DeviceType,
					ConnectedObjectsList = room.ConnectedObjectsList.OrderBy((arg) => (arg.DeviceType)).ToList<ConnectedObject>(),
					Sensors = room.SensorsList,
					StatusSensors = room.StatusSensors,
					Id = room.Id,
				})?.OrderByDescending((room) => room.ConnectedObjectsList?.Count);
			}
			catch (Exception ex) 
			{ 
                Log.Debug($"{ClassHelper.GetCallerClassAndMethodName()} - {ex.ToString()}");
                this.NavigationService.ShowMessage("Room Service Exception", ZaptoSeverity.Error);
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

using Connect.Application;
using Connect.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Zapto.Component.Common.Models;
using Zapto.Component.Common.Resources;

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
		public IStringLocalizer<Resource> Localizer { get; }

		#endregion

		#region Constructor
		public RoomListViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.ApplicationRoomServices = serviceProvider.GetRequiredService<IApplicationRoomServices>();
			this.Localizer = serviceProvider.GetRequiredService<IStringLocalizer<Resource>>();
		}
		#endregion

		#region Methods

		public async Task<IEnumerable<RoomModel>?> GetRoomModels(string? locationId)
		{
			return (await this.ApplicationRoomServices.GetRoomsAsync(locationId))?.Select((room) => new RoomModel()
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
		#endregion
	}
}

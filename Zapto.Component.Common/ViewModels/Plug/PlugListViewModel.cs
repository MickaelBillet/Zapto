﻿using Connect.Model;
using System.Diagnostics;
using Zapto.Component.Common.Models;
using Status = Connect.Model.Status;

namespace Zapto.Component.Common.ViewModels
{
    public interface IPlugListViewModel : IBaseViewModel
	{
        IEnumerable<PlugModel>? GetPlugModels(RoomModel roomModel);
    }

	public class PlugListViewModel : BaseViewModel, IPlugListViewModel
    {
        #region Properties
        #endregion

        #region Constructor
        public PlugListViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
        }
        #endregion

        #region Methods

		public IEnumerable<PlugModel>? GetPlugModels(RoomModel roomModel)
		{
            IEnumerable<PlugModel>? models = null;
			try
			{
				this.IsLoading = true;

				models = roomModel.ConnectedObjectsList?.Where((obj) => obj.Plug != null).Select((obj) => new PlugModel()
				{
					Name = obj.Name,
					LocationId = roomModel.LocationId,
					ConditionType = obj.Plug?.ConditionType,
					Id = obj.Plug?.Id,
					Status = (obj.Plug != null) ? Plug.GetStatus(obj.Plug.Status, obj.Plug.Order) : Status.OFF,
					WorkingDuration = (obj.Plug != null) ? obj.Plug.WorkingDuration : 0f,
					Command = (obj.Plug != null) ? Plug.GetCommand(obj.Plug.OnOff, obj.Plug.Mode) : CommandType.Off,
				}).ToList();
			}
			catch (Exception ex) 
			{
				Debug.WriteLine(ex);
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

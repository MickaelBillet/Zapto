using Connect.Application;
using Connect.Application.Infrastructure;
using Connect.Model;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface IPlugViewModel : IBaseViewModel
	{
		Task<bool?> SendCommandAsync(int? command, string? id);
        Task<bool> ReceiveStatusAsync(PlugModel model);
    }

    public class PlugViewModel : BaseViewModel, IPlugViewModel
    {
		#region Properties
		private IApplicationPlugServices ApplicationPlugServices { get; }
        private ISignalRService SignalRService { get; }
        #endregion

        #region Constructor
        public PlugViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.ApplicationPlugServices = serviceProvider.GetRequiredService<IApplicationPlugServices>();
            this.SignalRService = serviceProvider.GetRequiredService<ISignalRService>();
        }
        #endregion

        #region Methods

        public async Task<bool> ReceiveStatusAsync(PlugModel model)
        {
            bool res = false;
            try
            {
                res = await this.SignalRService.StartAsync(model.LocationId,
					(plugStatus) =>
					{
						if (model?.Id == plugStatus.PlugId)
						{
							model.Status = Plug.GetStatus(plugStatus.Status, plugStatus.Order);
							model.WorkingDuration = plugStatus.WorkingDuration;
							model.Command = Plug.GetCommand(plugStatus.OnOff, plugStatus.Mode);
							this.OnRefresh(new EventArgs());
						}
					},
					null,
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

        public async Task <bool?> SendCommandAsync(int? command, string? id)
		{
			bool? res = null;
			try
			{
				if (command == 1)
				{
					res = await this.ApplicationPlugServices.ChangeMode(new Plug()
					{
						Id = id,
						OnOff = Status.OFF,
						Mode = Mode.Manual,
					});
				}
				else if (command == 2)
				{
					res = await this.ApplicationPlugServices.ChangeMode(new Plug()
					{
						Id = id,
						OnOff = Status.ON,
						Mode = Mode.Manual,
					});
				}
				else if (command == 3)
				{
					res = await this.ApplicationPlugServices.ChangeMode(new Plug()
					{
						Id = id,
						OnOff = Status.ON,
						Mode = Mode.Programing,
					});
				}
			}
			catch (Exception ex)
			{
				Debug.Write(ex);
				throw new Exception("Send Plug Command Exception");
			}
			return res;
		}		

        public override void Dispose()
        {
            base.Dispose();
            this.SignalRService?.Dispose();
        }

        #endregion
    }
}

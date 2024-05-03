using Connect.Application;
using Connect.Application.Infrastructure;
using Connect.Model;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface IPlugViewModel : IBaseViewModel
	{
		Task<bool?> SendCommandAsync(PlugModel model);
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
                Log.Debug($"{ClassHelper.GetCallerClassAndMethodName()} - {ex.ToString()}");
                this.NavigationService.ShowMessage("SignalR Exception", ZaptoSeverity.Error);
            }
            return res;
        }

        public async Task <bool?> SendCommandAsync(PlugModel model)
		{
			bool? res = null;
			try
			{
				if (model.Command == CommandType.Off)
				{
					res = await this.ApplicationPlugServices.ChangeMode(new Plug()
					{
						Id = model.Id,
						OnOff = Status.OFF,
						Mode = Mode.Manual,
					});
				}
				else if (model.Command == CommandType.Manual)
				{
					res = await this.ApplicationPlugServices.ChangeMode(new Plug()
					{
						Id = model.Id,
						OnOff = Status.ON,
						Mode = Mode.Manual,
					});
				}
				else if (model.Command == CommandType.Programing)
				{
					res = await this.ApplicationPlugServices.ChangeMode(new Plug()
					{
						Id = model.Id,
						OnOff = Status.ON,
						Mode = Mode.Programing,
					});
				}
			}
			catch (Exception ex)
			{
                Log.Debug($"{ClassHelper.GetCallerClassAndMethodName()} - {ex.ToString()}");
                this.NavigationService.ShowMessage("Send Plug Command Exception", ZaptoSeverity.Error);
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

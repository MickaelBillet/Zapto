using Connect.Application.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface ISensorDataViewModel : IBaseViewModel
    {
        Task<bool> ReceiveStatusAsync(SensorDataModel model);
    }

    public class SensorDataViewModel : BaseViewModel, ISensorDataViewModel
    {
        #region Properties
        private ISignalRService SignalRService { get; }

        #endregion

        #region Constructor

        public SensorDataViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.SignalRService = serviceProvider.GetRequiredService<ISignalRService>();
        }

        #endregion

        #region Methods

        public override void Dispose()
        {
            base.Dispose();
            this.SignalRService?.Dispose();
        }

        public async Task<bool> ReceiveStatusAsync(SensorDataModel model)
        {
            bool res = false;
            try
            {
                res = await this.SignalRService.StartAsync(model.LocationId,
                        null,
                        null,
                        (sensorStatus) =>
                        {
                            if (sensorStatus.SensorId == model.Id)
                            {
                                this.OnRefresh(new EventArgs());
                            }
                        },
                        null);
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                throw new Exception("SignalR Exception");

            }
            return res;
        }

        #endregion
    }
}

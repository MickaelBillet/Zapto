using Connect.Application;
using Connect.Application.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface ISensorEventViewModel : IBaseViewModel
    {
        Task CancelLeakEvent(SensorEventModel? model);
        Task<bool> ReceiveStatusAsync(SensorEventModel model);
    }

    public class SensorEventViewModel : BaseViewModel, ISensorEventViewModel
    {
        #region Properties
        private IApplicationSensorServices ApplicationSensorServices { get; }
        private ISignalRService SignalRService { get; }
        #endregion

        #region Constructor

        public SensorEventViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.ApplicationSensorServices = serviceProvider.GetRequiredService<IApplicationSensorServices>();
            this.SignalRService = serviceProvider.GetRequiredService<ISignalRService>();
        }

        #endregion

        #region Methods

        public override void Dispose()
        {
            base.Dispose();
            this.SignalRService?.Dispose();
        }

        public async Task CancelLeakEvent(SensorEventModel? model)
        {
            if (model != null)
            {
                model.HasLeak = 0;

                if (await this.ApplicationSensorServices.Leak(model.Id, model.HasLeak) == true)
                {
                    this.OnRefresh(new EventArgs());
                }
            }
        }

        public async Task<bool> ReceiveStatusAsync(SensorEventModel model)
        {
            return await this.SignalRService.StartAsync(model.LocationId,
            null,
            null,
            (sensorStatus) =>
            {
                if (sensorStatus.SensorId == model.Id)
                {
                    if (sensorStatus.LeakDetected != 0)
                    {
                        model.HasLeak = (sensorStatus.LeakDetected != null) ? (int)sensorStatus.LeakDetected : 0;
                        this.OnRefresh(new EventArgs());
                    }
                }
            },
            null);
        }

        #endregion
    }
}

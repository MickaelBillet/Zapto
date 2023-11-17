using AirZapto.Application;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface ISensorCO2ViewModel : IBaseViewModel
    {
        Task<SensorCO2Model?> GetSensorCO2Model(string sensorName);
    }

    public class SensorCO2ViewModel : BaseViewModel, ISensorCO2ViewModel
    {
        #region Properties
        private IApplicationSensorServices ApplicationSensorServices { get; }
        #endregion

        #region Constructor
        public SensorCO2ViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.ApplicationSensorServices = serviceProvider.GetRequiredService<IApplicationSensorServices>();
        }
        #endregion

        #region Methods

        public override async Task InitializeAsync(string? parameter)
        {
            await base.InitializeAsync(parameter);
        }
        public async Task<SensorCO2Model?> GetSensorCO2Model(string sensorName)
        {
            SensorCO2Model? model = null;
            try
            {
                if (this.ApplicationSensorServices != null)
                {
                    model = (await this.ApplicationSensorServices.GetSensorsAsync())?.Select((sensor) => new SensorCO2Model()
                    {
                        Id = sensor.Id,
                        CO2 = sensor.CO2,
                        Description = sensor.Description,
                        Mode = sensor.Mode,
                        IsRunning = sensor.IsRunning,
                    })?.FirstOrDefault((arg) => (arg.Description != null) ? arg.Description.Equals(sensorName, StringComparison.Ordinal) : false);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            return model;
        }

        #endregion
    }
}

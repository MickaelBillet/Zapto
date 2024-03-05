using AirZapto.Application;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface ISensorCO2ViewModel : IBaseViewModel
    {
        Task<(SensorCO2Model? model, bool hasError)> GetSensorCO2Model(string sensorName);
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
        public async Task<(SensorCO2Model? model, bool hasError)> GetSensorCO2Model(string sensorName)
        {
            SensorCO2Model? model = null;
            bool hasError = false;

            try
            {
                this.IsLoading = true;

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
                Log.Debug($"{ClassHelper.GetCallerClassAndMethodName()} - {ex.ToString()}");
                hasError = true;
                this.NavigationService.ShowMessage("Sensor CO2 Service Exception", ZaptoSeverity.Error);
            }
            finally
            {
                this.IsLoading = false;
            }

            return (model, hasError);
        }

        #endregion
    }
}

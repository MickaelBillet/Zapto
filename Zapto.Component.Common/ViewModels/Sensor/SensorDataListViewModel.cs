﻿using Connect.Model;
using System.Diagnostics;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface ISensorDataListViewModel : IBaseViewModel
    {
        IEnumerable<SensorDataModel>? GetSensorDataModels(RoomModel? roomModel);
    }

    public class SensorDataListViewModel : BaseViewModel, ISensorDataListViewModel
    {
        #region Properties
        #endregion

        #region Constructor
        public SensorDataListViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        #endregion

        #region Methods

        public IEnumerable<SensorDataModel>? GetSensorDataModels(RoomModel? roomModel)
        {
            IEnumerable<SensorDataModel>? models = null;
            try
            {
                models = roomModel?.ConnectedObjectsList?.Where((obj) => (obj.Sensor != null)
                                                                        && (((obj.Sensor.Type & DeviceType.Sensor_Humidity) == DeviceType.Sensor_Humidity)
                                                                            || ((obj.Sensor.Type & DeviceType.Sensor_Pressure) == DeviceType.Sensor_Pressure)
                                                                            || ((obj.Sensor.Type & DeviceType.Sensor_Temperature) == DeviceType.Sensor_Temperature)))
                                                            .Select((obj) => new SensorDataModel()
                                                                                {
                                                                                    Id = obj.Id,
                                                                                    LocationId = roomModel.LocationId,
                                                                                    Name = obj.Name,
                                                                                    Temperature = obj.Sensor?.Temperature?.ToString("0.0"),
                                                                                    IsRunning = (obj.Sensor != null) ? obj.Sensor.IsRunning : (byte)0,
                                                                                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return models;
        }
        #endregion
    }
}

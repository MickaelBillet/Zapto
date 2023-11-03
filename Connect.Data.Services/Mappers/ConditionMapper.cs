using Connect.Data.Entities;
using Connect.Model;

namespace Connect.Data.Mappers
{
    internal static class ConditionMapper
    {
        public static ConditionEntity Map(Condition model)
        {
            ConditionEntity entity = new ConditionEntity()
            {
                CreationDateTime = model.Date,
                Id = model.Id,
                ConnectedObjectId = model.ConnectedObjectId,
                HumidityOrder = model.HumidityOrder,
                HumidityOrderIsEnabled = model.HumidityOrderIsEnabled,
                OperationRangetId = model.OperationRangetId,
                TemperatureOrder = model.TemperatureOrder,
                TemperatureOrderIsEnabled = model.TemperatureOrderIsEnabled,
            };
            return entity;
        }

        public static Condition Map(ConditionEntity entity) 
        {
            Condition model = new Condition()
            {
                Date = entity.CreationDateTime,
                Id = entity.Id,
                ConnectedObjectId = entity.ConnectedObjectId,
                HumidityOrder = entity.HumidityOrder,
                HumidityOrderIsEnabled = entity.HumidityOrderIsEnabled,
                OperationRangetId = entity.OperationRangetId,
                TemperatureOrder = entity.TemperatureOrder,
                TemperatureOrderIsEnabled = entity.TemperatureOrderIsEnabled,

            };
            return model;
        }
    }
}

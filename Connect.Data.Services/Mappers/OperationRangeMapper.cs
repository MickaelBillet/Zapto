using Connect.Data.Entities;
using Connect.Model;

namespace Connect.Data.Mappers
{
    internal static class OperationRangeMapper
    {
        public static OperationRangeEntity Map(OperationRange model)
        {
            OperationRangeEntity entity = new OperationRangeEntity()
            {
                CreationDateTime = model.Date,
                Id = model.Id,
                AllDay = model.AllDay,
                ConditionId = model.ConditionId,
                Day = model.Day,
                EndTime = model.EndTime,
                ProgramId = model.ProgramId,
                StartTime = model.StartTime,
            };
            return entity;
        }

        public static OperationRange Map(OperationRangeEntity entity) 
        {
            OperationRange model = new OperationRange()
            {
                Date = entity.CreationDateTime,
                Id = entity.Id,
                AllDay = entity.AllDay,
                ConditionId = entity.ConditionId,
                Day = entity.Day,
                EndTime = entity.EndTime,
                ProgramId = entity.ProgramId,
                StartTime = entity.StartTime,
            };
            return model;
        }
    }
}

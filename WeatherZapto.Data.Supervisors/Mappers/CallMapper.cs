using WeatherZapto.Data.Entities;
using WeatherZapto.Model;

namespace WeatherZapto.Data.Mappers
{
    internal static class CallMapper
    {
        public static CallEntity Map(Call model)
        {
            CallEntity entity = new CallEntity()
            {
                CreationDateTime = model.Date,
                Id = model.Id,
                Count = model.Count,
            };
            return entity;
        }

        public static Call Map(CallEntity entity)
        {
            Call model = new Call()
            {
                Date = entity.CreationDateTime,
                Id = entity.Id,
                Count = entity.Count,
            };
            return model;
        }
    }
}

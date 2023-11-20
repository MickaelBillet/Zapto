using Framework.Core.Domain;
using WeatherZapto.Data.Entities;

namespace WeatherZapto.Data.Mappers
{
    internal static class LogsMapper
    {
        public static LogsEntity Map(Logs model)
        {
            LogsEntity entity = new LogsEntity()
            {
                CreationDateTime = model.Date,
                Id = model.Id,
                Exception = model.Exception,
                Level = model.Level,
                Properties = model.Properties,
                RenderedMessage = model.RenderedMessage,
            };
            return entity;
        }

        public static Logs Map(LogsEntity entity)
        {
            Logs model = new Logs()
            {
                Date = entity.CreationDateTime,
                Id = entity.Id,
                Exception = entity.Exception,
                Level = entity.Level,
                Properties = entity.Properties,
                RenderedMessage = entity.RenderedMessage,
            };
            return model;
        }
    }
}

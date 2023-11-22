using AirZapto.Data.Entities;
using Framework.Core.Domain;

namespace AirZapto.Data.Mappers
{
    internal static class LogsMapper
    {
        public static LogsEntity Map(Logs data)
        {
            LogsEntity entity = new LogsEntity()
            {
                Id = data.Id,
                Exception = data.Exception ?? string.Empty,
                Level= data.Level ?? string.Empty,
                Properties= data.Properties ?? string.Empty,    
                RenderedMessage= data.RenderedMessage ?? string.Empty,
                CreationDateTime = data.Date,
            };
            return entity;
        }

        public static Logs Map(LogsEntity entity)
        {
            Logs data = new Logs()
            {
                Id = entity.Id,
                Exception = entity.Exception,
                Level = entity.Level,
                Properties = entity.Properties,
                RenderedMessage = entity.RenderedMessage,
                Date = entity.CreationDateTime,
            };
            return data;
        }
    }
}

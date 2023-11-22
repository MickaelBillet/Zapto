using Connect.Data.Entities;
using Connect.Model;

namespace Connect.Data.Mappers
{
    internal static class ConfigurationMapper
    {
        public static ConfigurationEntity Map(Configuration model)
        {
            ConfigurationEntity entity = new ConfigurationEntity()
            {
                CreationDateTime = model.Date,
                Id = model.Id,
                Address = model.Address,
                Period = model.Period,
                Pin0 = model.Pin0,
                ProtocolType = model.ProtocolType,
                Unit = model.Unit,  
            };
            return entity;
        }

        public static Configuration Map(ConfigurationEntity entity) 
        {
            Configuration model = new Configuration()
            {
                Date = entity.CreationDateTime,
                Id = entity.Id,
                Address = entity.Address,
                Period = entity.Period,
                Pin0 = entity.Pin0,
                ProtocolType = entity.ProtocolType,
                Unit = entity.Unit,
            };
            return model;
        }
    }
}

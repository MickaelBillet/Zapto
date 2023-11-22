using Connect.Data.Entities;
using Connect.Model;

namespace Connect.Data.Mappers
{
    internal static class ClientAppMapper
    {
        public static ClientAppEntity Map(ClientApp model)
        {
            ClientAppEntity entity = new ClientAppEntity()
            {
                CreationDateTime = model.Date,
                Description = model.Description,
                Id = model.Id,
                LocationId = model.LocationId,
                Token = model.Token,
            };
            return entity;
        }

        public static ClientApp Map(ClientAppEntity entity) 
        {
            ClientApp model = new ClientApp()
            {
                Date = entity.CreationDateTime,
                Description = entity.Description,
                Id = entity.Id,
                LocationId = entity.LocationId,
                Token = entity.Token,
            };
            return model;
        }
    }
}

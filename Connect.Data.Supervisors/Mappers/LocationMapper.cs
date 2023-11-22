using Connect.Data.Entities;
using Connect.Model;

namespace Connect.Data.Mappers
{
    internal static class LocationMapper
    {
        public static LocationEntity Map(Location model)
        {
            LocationEntity entity = new LocationEntity()
            {
                CreationDateTime = model.Date,
                Id = model.Id,
                Address = model.Address,
                City = model.City,
                Country = model.Country,
                Description = model.Description,
                UserId = model.UserId,
                ZipCode = model.Zipcode,
            };
            return entity;
        }

        public static Location Map(LocationEntity entity) 
        {
            Location model = new Location()
            {
                Date = entity.CreationDateTime,
                Id = entity.Id,
                Address = entity.Address,
                City = entity.City,
                Country = entity.Country,
                Description = entity.Description,
                UserId = entity.UserId,
                Zipcode = entity.ZipCode,
            };
            return model;
        }
    }
}

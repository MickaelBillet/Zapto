using Connect.Data.Entities;
using Connect.Model;

namespace Connect.Data.Mappers
{
    internal static class ProgramMapper
    {
        public static ProgramEntity Map(Program model)
        {
            ProgramEntity entity = new ProgramEntity()
            {
                CreationDateTime = model.Date,
                Id = model.Id,
                ConnectedObjectId = model.ConnectedObjectId,
                
            };
            return entity;
        }

        public static Program Map(ProgramEntity entity) 
        {
            Program model = new Program()
            {
                Date = entity.CreationDateTime,
                Id = entity.Id,
                ConnectedObjectId= entity.ConnectedObjectId,
            };
            return model;
        }
    }
}

using Framework.Core.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirZapto.Data.Entities
{
    [Table("Version")]
    public class VersionEntity : ItemEntity
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }
        public int Revision { get; set; }
    }
}

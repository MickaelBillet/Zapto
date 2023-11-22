using Framework.Core.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect.Data.Entities
{
    [Table("ServerIotStatus")]
    public class ServerIotStatusEntity : ItemEntity
    {
        public DateTime? ConnectionDate
        {
            get; set;
        }

        [MaxLength(16)]
        public string IpAddress
        {
            get; set;
        }
    }
}

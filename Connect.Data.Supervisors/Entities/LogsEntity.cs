using Framework.Core.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect.Data.Entities
{
    [Table("Logs")]
    public class LogsEntity : ItemEntity
    {
        public string Level { get; set; }
        public string Exception { get; set; }
        public string RenderedMessage { get; set; }
        public string Properties { get; set; }
    }
}

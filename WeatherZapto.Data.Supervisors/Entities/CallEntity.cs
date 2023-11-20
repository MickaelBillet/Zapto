using Framework.Core.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherZapto.Data.Entities
{
    [Table("Call")]
    public class CallEntity : ItemEntity
    {
        public int Count { get; set; }
    }
}

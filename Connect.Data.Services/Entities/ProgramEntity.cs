using Framework.Core.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect.Data.Entities
{
    [Table("Program")]
    public class ProgramEntity : ItemEntity
    {
        [Required] public string ConnectedObjectId { get; set; } = string.Empty;
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Framework.Core.Data
{
    public abstract class ItemEntity
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required]
        public DateTime CreationDateTime { get; set; }
    }
}

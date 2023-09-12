using ParentApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParentApi.Models
{
    [Table("classes", Schema = "skool")]
    public class Class : EntityBase
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public char Letter { get; set; }
    }
}

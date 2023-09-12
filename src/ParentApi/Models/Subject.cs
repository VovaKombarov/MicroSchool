using ParentApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParentApi.Models
{
    [Table("subjects", Schema = "skool")]
    public class Subject : EntityBase
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string SubjectName { get; set; }
    }
}

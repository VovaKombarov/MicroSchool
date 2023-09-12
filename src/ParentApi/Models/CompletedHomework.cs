using ParentApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParentApi.Models
{
    [Table("completedhomeworks", Schema = "skool")]
    public class CompletedHomework : EntityBase
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public StudentInLesson StudentInLesson { get; set; }

        public string Work { get; set; }

        public int? Grade { get; set; }
    }
}

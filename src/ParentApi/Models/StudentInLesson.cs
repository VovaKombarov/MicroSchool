using ParentApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParentApi.Models
{
    [Table("studentsinlessons", Schema = "skool")]
    public class StudentInLesson : EntityBase
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public Student Student { get; set; }

        [Required]
        public Lesson Lesson { get; set; }

        public string Comment { get; set; }

        public int? Grade { get; set; }
    }
}

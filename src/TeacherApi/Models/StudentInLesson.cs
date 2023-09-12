using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TeacherApi.Data;

namespace TeacherApi.Models
{
    [Table("StudentsInLessons", Schema = "skool")]
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

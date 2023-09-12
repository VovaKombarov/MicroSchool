using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeacherApi.Data;

namespace TeacherApi.Models
{
    [Table("Lessons", Schema = "skool")]
    public class Lesson : EntityBase
    {
        [Key]
        [Required]
        public int Id { get; set; }

      
        [Required]
        public TeacherClassSubject TeacherClassSubject { get; set; }

        [Required]
        public DateTime LessonDT { get; set; }

        [Required]
        public string Theme { get; set; }

    }
}

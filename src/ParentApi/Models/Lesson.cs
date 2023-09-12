using ParentApi.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParentApi.Models
{
    [Table("lessons", Schema = "skool")]
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

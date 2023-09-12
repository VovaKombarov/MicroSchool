using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeacherApi.Data;

namespace TeacherApi.Models
{
    [Table("Homeworks", Schema = "skool")]
    public class Homework : EntityBase
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public Lesson Lesson { get; set; }

        [Required]
        public DateTime StartDT { get; set; }

        [Required]
        public DateTime FinishDT { get; set; }

        [Required]
        public string Howework { get; set; }
    }
}

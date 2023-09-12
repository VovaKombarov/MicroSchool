using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeacherApi.Data;

namespace TeacherApi.Models
{
    [Table("HomeworkProgressStatuses", Schema = "skool")]
    public class HomeworkProgressStatus : EntityBase
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public StudentInLesson StudentInLesson { get; set; }

        [Required]
        public DateTime StatusSetDT { get; set; }

        [Required]
        public HomeworkStatus HomeworkStatus { get; set; }


    }
}

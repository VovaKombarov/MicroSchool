using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TeacherApi.Data;

namespace TeacherApi.Models
{
    [Table("TeachersParentsMeetings", Schema = "skool")]
    public class TeacherParentMeeting : EntityBase
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public Parent Parent { get; set; }

        [Required]
        public Teacher Teacher { get; set; }

        [Required]
        public Student Student { get; set; }

        [Required]
        public DateTime MeetingDT { get; set; }

        public StudentInLesson StudentInLesson { get; set; }

        [Required]
        public bool TeacherInitiative { get; set; }
    }
}

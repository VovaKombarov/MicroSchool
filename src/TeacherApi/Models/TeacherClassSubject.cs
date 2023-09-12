using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TeacherApi.Data;

namespace TeacherApi.Models
{
    [Table("TeachersClassesSubjects", Schema = "skool")]
    public class TeacherClassSubject : EntityBase
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public Teacher Teacher { get; set; }

        [Required]
        public Class Class { get; set; }

        [Required]
        public Subject Subject { get; set; }
    }
}

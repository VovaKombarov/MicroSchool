using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TeacherApi.Data;

namespace TeacherApi.Models
{
    [Table("Subjects", Schema = "skool")]
    public class Subject : EntityBase
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string SubjectName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TeacherApi.Data;

namespace TeacherApi.Models
{
    [Table("HomeworkStatuses", Schema = "skool")]
    public class HomeworkStatus : EntityBase
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Status { get; set; }
    }
}

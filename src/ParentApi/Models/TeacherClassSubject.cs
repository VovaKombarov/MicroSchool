using ParentApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParentApi.Models
{
    [Table("teachersclassessubjects", Schema = "skool")]
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

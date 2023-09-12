using ParentApi.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParentApi.Models
{
    [Table("homeworks", Schema = "skool")]
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

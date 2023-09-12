using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ParentApi.Data;

namespace ParentApi.Models
{
    [Table("homeworkstatuses", Schema = "skool")]
    public class HomeworkStatus : EntityBase
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Status { get; set; }
    }
}


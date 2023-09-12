using ParentApi.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParentApi.Models
{
    [Table("parents", Schema = "skool")]
    public class Parent : EntityBase
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Patronymic { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public IEnumerable<Student> Students { get; set; }

    }
}

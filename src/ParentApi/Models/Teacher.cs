using ParentApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParentApi.Models
{
    [Table("teachers", Schema = "skool")]
    public class Teacher : EntityBase
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

    }
}

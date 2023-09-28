using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TeacherApi.Data;

namespace TeacherApi.Models
{
    [Table("Teachers", Schema = "skool")]
    public class Teacher : EntityBase
    {
        #region Properties

        /// <summary>
        /// Идентификатор учителя.
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Имя учителя.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Отчество учителя.
        /// </summary>
        [Required]
        public string Patronymic { get; set; }

        /// <summary>
        /// Фамилия учителя.
        /// </summary>
        [Required]
        public string Surname { get; set; }

        #endregion Properties
    }
}

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
        #region Properties

        /// <summary>
        /// Идентификатор предмета.
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Имя предмета.
        /// </summary>
        [Required]
        public string SubjectName { get; set; }

        #endregion Properties
    }
}

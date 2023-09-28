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
        #region Properties

        /// <summary>
        /// Идентификатор статуса домашней работы.
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Статус домашней работы.
        /// </summary>
        [Required]
        public string Status { get; set; }

        #endregion Properties
    }
}

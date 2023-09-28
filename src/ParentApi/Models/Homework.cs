using ParentApi.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParentApi.Models
{
    /// <summary>
    /// Домашняя работа.
    /// </summary>
    [Table("homeworks", Schema = "skool")]
    public class Homework : EntityBase
    {
        #region Properties

        /// <summary>
        /// Идентификатор домашней работы.
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Урок.
        /// </summary>
        [Required]
        public Lesson Lesson { get; set; }

        /// <summary>
        /// Время начала урока.
        /// </summary>
        [Required]
        public DateTime StartDT { get; set; }

        /// <summary>
        /// Время окончания урока.
        /// </summary>
        [Required]
        public DateTime FinishDT { get; set; }

        /// <summary>
        /// Домашняя работа заданная на уроке.
        /// </summary>
        [Required]
        public string Howework { get; set; }

        #endregion Properties
    }
}

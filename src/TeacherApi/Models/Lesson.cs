using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeacherApi.Data;

namespace TeacherApi.Models
{
    [Table("Lessons", Schema = "skool")]
    public class Lesson : EntityBase
    {
        #region Properties

        /// <summary>
        /// Идентификатор урока.
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Обьект связывающий учителя/класс/предмет
        /// </summary>
        [Required]
        public TeacherClassSubject TeacherClassSubject { get; set; }

        /// <summary>
        /// Время урока.
        /// </summary>
        [Required]
        public DateTime LessonDT { get; set; }

        /// <summary>
        /// Тема урока.
        /// </summary>
        [Required]
        public string Theme { get; set; }

        #endregion Properties 
    }
}

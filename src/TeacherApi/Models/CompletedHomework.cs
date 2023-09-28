using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeacherApi.Data;

namespace TeacherApi.Models
{
    [Table("CompletedHomeworks", Schema = "skool")]
    public class CompletedHomework : EntityBase
    {
        #region Properties 

        /// <summary>
        /// Идентификатор домашней работы.
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Студент на уроке.
        /// </summary>
        [Required]
        public StudentInLesson StudentInLesson { get; set; }

        /// <summary>
        /// Домашняя работа.
        /// </summary>
        public string Work { get; set; }

        /// <summary>
        /// Оценка.
        /// </summary>
        public int? Grade { get; set; }

        #endregion Properties
    }

}

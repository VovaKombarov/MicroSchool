using ParentApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParentApi.Models
{
    /// <summary>
    /// Студент на уроке.
    /// </summary>
    [Table("studentsinlessons", Schema = "skool")]
    public class StudentInLesson : EntityBase
    {
        #region Properties

        /// <summary>
        /// Идентификатор студента на уроке.
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Студент.
        /// </summary>
        [Required]
        public Student Student { get; set; }

        /// <summary>
        /// Урок.
        /// </summary>
        [Required]
        public Lesson Lesson { get; set; }

        /// <summary>
        /// Замечание полученное на уроке.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Оценка полученная на уроке.
        /// </summary>
        public int? Grade { get; set; }

        #endregion Properties
    }
}

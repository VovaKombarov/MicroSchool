using ParentApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParentApi.Models
{
    /// <summary>
    /// Связка учитель/класс/предмет
    /// </summary>
    [Table("teachersclassessubjects", Schema = "skool")]
    public class TeacherClassSubject : EntityBase
    {
        #region Properties

        /// <summary>
        /// Идентификатор.
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Учитель.
        /// </summary>
        [Required]
        public Teacher Teacher { get; set; }

        /// <summary>
        /// Класс.
        /// </summary>
        [Required]
        public Class Class { get; set; }

        /// <summary>
        /// Предмет.
        /// </summary>
        [Required]
        public Subject Subject { get; set; }

        #endregion Properties
    }
}

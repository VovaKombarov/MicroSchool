using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeacherApi.Data;

namespace TeacherApi.Models
{
    /// <summary>
    /// Класс.
    /// </summary>
    [Table("classes", Schema = "skool")]
    public class Class : EntityBase
    {
        #region Properties

        /// <summary>
        /// Идентификатор класса.
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Номер класса.
        /// </summary>
        [Required]
        public int Number { get; set; }

        /// <summary>
        /// Буква класса.
        /// </summary>
        [Required]
        public char Letter { get; set; }

        #endregion Properties
    }
}

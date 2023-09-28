using ParentApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParentApi.Models
{
    /// <summary>
    /// Учитель.
    /// </summary>
    [Table("teachers", Schema = "skool")]
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

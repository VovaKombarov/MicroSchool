using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeacherApi.Data;

namespace TeacherApi.Models
{
    [Table("Parents", Schema = "skool")]
    public class Parent : EntityBase
    {
        #region Properties

        /// <summary>
        /// Идентификатор родителя.
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Имя родителя.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Отчество родителя.
        /// </summary>
        [Required]
        public string Patronymic { get; set; }

        /// <summary>
        /// Фамилия родителя.
        /// </summary>
        [Required]
        public string Surname { get; set; }

        /// <summary>
        /// Коллекция студентов, которые относятся к родителю (дети).
        /// </summary>
        [Required]
        public IEnumerable<Student> Students { get; set; }

        #endregion Properties
    }
}

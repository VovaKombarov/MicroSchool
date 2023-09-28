using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ParentApi.Data;

namespace ParentApi.Models
{
    /// <summary>
    /// Статус домашней работы.
    /// </summary>
    [Table("homeworkstatuses", Schema = "skool")]
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


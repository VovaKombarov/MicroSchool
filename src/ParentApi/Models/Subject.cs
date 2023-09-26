using ParentApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParentApi.Models
{
    /// <summary>
    /// Предмет.
    /// </summary>
    [Table("subjects", Schema = "skool")]
    public class Subject : EntityBase
    {
        /// <summary>
        /// Идентификатор предмета.
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Имя предмета.
        /// </summary>
        [Required]
        public string SubjectName { get; set; }
    }
}

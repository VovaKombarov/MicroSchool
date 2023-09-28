using ParentApi.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParentApi.Models
{
    /// <summary>
    /// Встреча учителя и родителя.
    /// </summary>
    [Table("teachersparentsmeetings", Schema = "skool")]
    public class TeacherParentMeeting : EntityBase
    {
        #region Properties

        /// <summary>
        /// Идентификатор.
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Родитель.
        /// </summary>
        [Required]
        public Parent Parent { get; set; }

        /// <summary>
        /// Учитель.
        /// </summary>
        [Required]
        public Teacher Teacher { get; set; }

        /// <summary>
        /// Студент.
        /// </summary>
        [Required]
        public Student Student { get; set; }

        /// <summary>
        /// Время митинга.
        /// </summary>
        [Required]
        public DateTime MeetingDT { get; set; }

        /// <summary>
        /// Студент на уроке.
        /// </summary>
        public StudentInLesson StudentInLesson { get; set; }

        /// <summary>
        /// Признак того, что инициатором встречи был учитель.
        /// </summary>
        [Required]
        public bool TeacherInitiative { get; set; }

        #endregion Properties
    }

}

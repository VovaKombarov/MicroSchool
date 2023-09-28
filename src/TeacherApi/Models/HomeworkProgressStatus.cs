﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeacherApi.Data;

namespace TeacherApi.Models
{
    [Table("HomeworkProgressStatuses", Schema = "skool")]
    public class HomeworkProgressStatus : EntityBase
    {
        #region Properties

        /// <summary>
        /// Идентификатор прогресса домашней работы.
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
        /// Время установки статуса прогресса домашней работы.
        /// </summary>
        [Required]
        public DateTime StatusSetDT { get; set; }

        /// <summary>
        /// Статус домашней работы.
        /// </summary>
        [Required]
        public HomeworkStatus HomeworkStatus { get; set; }

        #endregion Properties

    }
}

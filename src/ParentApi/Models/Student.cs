using ParentApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParentApi.Models
{
    /// <summary>
    /// Студент.
    /// </summary>
    [Table("students", Schema = "skool")]
    public class Student : EntityBase
    {
        #region Properties

        /// <summary>
        /// Идентификатор студента.
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Имя студента.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Отчество студента.
        /// </summary>
        [Required]
        public string Patronymic { get; set; }

        /// <summary>
        /// Фамилия студента.
        /// </summary>
        [Required]
        public string Surname { get; set; }

        /// <summary>
        /// День рождения студента.
        /// </summary>
        [Required]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Класс студента.
        /// </summary>
        [Required]
        public Class Class { get; set; }

        /// <summary>
        /// Коллекция родителей.
        /// </summary>
        [Required]
        public IEnumerable<Parent> Parents { get; set; }

        #endregion Properties
    }
}

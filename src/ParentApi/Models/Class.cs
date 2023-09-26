﻿using ParentApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParentApi.Models
{
    /// <summary>
    /// Класс.
    /// </summary>
    [Table("classes", Schema = "skool")]
    public class Class : EntityBase
    {
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
    }
}

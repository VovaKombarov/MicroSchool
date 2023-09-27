﻿namespace TeacherApi.Data.Dtos
{
    /// <summary>
    /// Готовая работа.
    /// </summary>
    public record CompletedHomeworkReadDto
    {
        #region Properties 

        /// <summary>
        /// Готовая работа.
        /// </summary>
        public string Work { get; init; }

        #endregion Properties

    }
}

namespace TeacherApi.Data.Dtos
{
    /// <summary>
    /// Родитель.
    /// </summary>
    public record ParentReadDto
    {
        #region Properties 

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Отчество.
        /// </summary>
        public string Patronymic { get; init; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string Surname { get; init; }

        #endregion Properties
    }
}

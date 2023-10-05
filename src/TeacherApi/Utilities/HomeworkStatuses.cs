namespace TeacherApi.Utilities
{
    /// <summary>
    /// Статусы домашней работы.
    /// </summary>
    public enum HomeworkStatuses
    {
        /// <summary>
        /// Назначена.
        /// </summary>
        Appointed = 1,

        /// <summary>
        /// В процессе.
        /// </summary>
        InProgress = 2,

        /// <summary>
        /// Готова.
        /// </summary>
        Completed = 3,

        /// <summary>
        /// На проверке учителя.
        /// </summary>
        TeacherCheck = 4,

        /// <summary>
        /// На проверке у родителя.
        /// </summary>
        ParentCheck = 5,

        /// <summary>
        /// Оценена.
        /// </summary>
        Rated = 6,

    }
}

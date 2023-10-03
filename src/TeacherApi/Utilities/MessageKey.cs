namespace TeacherApi.Utilities
{
    /// <summary>
    /// Перечисление для типов сообщения.
    /// </summary>
    public enum MessageKey
    {
        /// <summary>
        /// Статус домашней работы некорректный.
        /// </summary>
        HomeworkStatusNotValid,

        /// <summary>
        /// Оценка не валидна.
        /// </summary>
        GradeNotValid,

        /// <summary>
        /// Время невалидно.
        /// </summary>
        DateTimeNotValid,

        /// <summary>
        /// Замечание отсутсвует.
        /// </summary>
        CommentMissing
    }
}


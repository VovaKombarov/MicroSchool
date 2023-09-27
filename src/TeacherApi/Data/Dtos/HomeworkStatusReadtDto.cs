namespace TeacherApi.Data.Dtos
{
    /// <summary>
    /// Статус домашней работы.
    /// </summary>
    public record HomeworkStatusReadtDto
    {
        #region Properties 

        /// <summary>
        /// Статус домашней работы.
        /// </summary>
        public string Status { get; set; }

        #endregion Properties

    }
}

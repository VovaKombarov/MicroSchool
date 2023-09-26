using Common.EventBus;

namespace ParentApi.IntegrationEvents.Events
{
    /// <summary>
    /// Событие изменения статуса домашней работы.
    /// </summary>
    public class ChangeStatusHomeworkEvent : IntegrationEvent
    {
        #region Properties

        /// <summary>
        /// Наименование события интеграции.
        /// </summary>
        public string Name => typeof(ChangeStatusHomeworkEvent).Name;

        /// <summary>
        /// Идентификатор студента.
        /// </summary>
        public int StudentId { get;}

        /// <summary>
        /// Идентификатор урока.
        /// </summary>
        public int LessonId { get; }

        /// <summary>
        /// Статус домашней работы.
        /// </summary>
        public int HomeworkStatusId { get; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <param name="homeworkStatusId">Идентификатор статуса домашней работы.</param>
        public ChangeStatusHomeworkEvent(
            int studentId, int lessonId, int homeworkStatusId)
        {
            StudentId = studentId;
            LessonId = lessonId;
            HomeworkStatusId = homeworkStatusId;
        }

        #endregion Constructors
    }
}

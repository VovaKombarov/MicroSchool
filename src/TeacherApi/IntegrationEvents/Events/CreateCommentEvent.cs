using Common.EventBus;

namespace TeacherApi.IntegrationEvents.Events
{
    /// <summary>
    /// Событие создания замечания.
    /// </summary>
    public class CreateCommentEvent : IntegrationEvent
    {
        #region Properties 

        /// <summary>
        /// Наименование события.
        /// </summary>
        public string Name => typeof(CreateCommentEvent).Name;

        /// <summary>
        /// Идентификатор студента.
        /// </summary>
        public int StudentId { get; }

        /// <summary>
        /// Идентификатор урока.
        /// </summary>
        public int LessonId { get; }

        /// <summary>
        /// Замечание.
        /// </summary>
        public string Comment { get; }

        #endregion Properties

        #region Constructors 

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <param name="comment">Замечание.</param>
        public CreateCommentEvent(int studentId, int lessonId, string comment)
        {
            StudentId = studentId;
            LessonId = lessonId;
            Comment = comment;
        }

        #endregion Constructors

    }
}

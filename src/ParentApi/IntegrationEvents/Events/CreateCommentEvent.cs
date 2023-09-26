using Common.EventBus;

namespace ParentApi.IntegrationEvents.Events
{
    /// <summary>
    /// Событие создания замечания.
    /// </summary>
    public class CreateCommentEvent : IntegrationEvent
    {
        #region Properties

        /// <summary>
        /// наименование события.
        /// </summary>
        public string Name => typeof(CreateCommentEvent).Name;

        /// <summary>
        /// Идентификор студента.
        /// </summary>
        public int StudentId { get;}

        /// <summary>
        /// идентифкатор урока.
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
        /// <param name="comment">Заменчание.</param>
        public CreateCommentEvent(int studentId,  int lessonId, string comment)
        {
            StudentId = studentId; 
            LessonId = lessonId;
            Comment = comment;
        }

        #endregion Constructors
    }
}

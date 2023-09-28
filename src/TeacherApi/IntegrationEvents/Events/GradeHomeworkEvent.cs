using Common.EventBus;

namespace TeacherApi.IntegrationEvents.Events
{
    /// <summary>
    /// Событие оценивание домашней работы.
    /// </summary>
    public class GradeHomeworkEvent : IntegrationEvent
    {
        #region Properties 

        /// <summary>
        /// Наименование события.
        /// </summary>
        public string Name => typeof(GradeHomeworkEvent).Name;

        /// <summary>
        /// Идентификатор студента.
        /// </summary>
        public int StudentId { get; }

        /// <summary>
        /// Идентификатор урока.
        /// </summary>
        public int LessonId { get; }

        /// <summary>
        /// Оценка.
        /// </summary>
        public int Grade { get; }

        #endregion Properties

        #region Constructors 

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <param name="grade">Оценка.</param>
        public GradeHomeworkEvent(int studentId, int lessonId, int grade)
        {
            StudentId = studentId;
            LessonId = lessonId;
            Grade = grade;
        }

        #endregion Constructors
    }
}

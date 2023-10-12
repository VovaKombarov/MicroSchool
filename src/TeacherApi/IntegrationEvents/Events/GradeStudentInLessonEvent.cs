using Common.EventBus;

namespace TeacherApi.IntegrationEvents.Events
{
    /// <summary>
    /// Событие оценивания студента на уроке.
    /// </summary>
    public class GradeStudentInLessonEvent : IntegrationEvent
    {
        #region Properties 

        /// <summary>
        /// Наименование события.
        /// </summary>
        public string Name => typeof(GradeStudentInLessonEvent).Name;

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
        public GradeStudentInLessonEvent(int studentId, int lessonId, int grade)
        {
            StudentId = studentId;
            LessonId = lessonId;
            Grade = grade;
        }

        #endregion Constructors
    }
}

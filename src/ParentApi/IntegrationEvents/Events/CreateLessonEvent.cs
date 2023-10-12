using Common.EventBus;
using System;

namespace ParentApi.IntegrationEvents.Events
{
    /// <summary>
    /// Событие содания урока.
    /// </summary>
    public class CreateLessonEvent : IntegrationEvent
    {
        #region Properties

        /// <summary>
        /// Наименование события.
        /// </summary>
        public string Name => typeof(CreateLessonEvent).Name;

        /// <summary>
        /// Идентификатор учителя.
        /// </summary>
        public int TeacherId { get; }

        /// <summary>
        /// Идентификатор класса.
        /// </summary>
        public int ClassId { get; }

        /// <summary>
        /// Идентификатор предмета.
        /// </summary>
        public int SubjectId { get; }

        /// <summary>
        /// Тема урока.
        /// </summary>
        public string Theme { get; }

        /// <summary>
        /// Время урока.
        /// </summary>
        public DateTime LessonDateTime { get; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <param name="classId">Идентификатор класса.</param>
        /// <param name="subjectId">Идентификатор предмета.</param>
        /// <param name="theme">Тема.</param>
        /// <param name="lessonDateTime">Время урока.</param>
        public CreateLessonEvent(
            int teacherId,
            int classId,
            int subjectId,
            string theme,
            DateTime lessonDateTime)
        {
            TeacherId = teacherId;
            ClassId = classId;
            SubjectId = subjectId;
            Theme = theme;
            LessonDateTime = lessonDateTime;
        }

        #endregion Constructors
    }
}

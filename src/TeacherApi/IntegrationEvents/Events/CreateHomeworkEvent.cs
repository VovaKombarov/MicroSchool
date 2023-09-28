using Common.EventBus;
using System;

namespace TeacherApi.IntegrationEvents.Events
{
    /// <summary>
    /// Событие создания домашней работы.
    /// </summary>
    public class CreateHomeworkEvent : IntegrationEvent
    {
        #region Properties 

        /// <summary>
        /// Наименование события.
        /// </summary>
        public string Name => typeof(CreateHomeworkEvent).Name;

        /// <summary>
        /// Идентификатор урока.
        /// </summary>
        public int LessonId { get; }

        /// <summary>
        /// Срок окончания домашней работы.
        /// </summary>
        public DateTime FinishDateTime { get; }

        /// <summary>
        /// Домашняя работа.
        /// </summary>
        public string Homework { get; }

        #endregion Properties

        #region Constructors 

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <param name="finishDateTime">Время окончания домашней работы.</param>
        /// <param name="homeWork">Домашняя работа.</param>
        public CreateHomeworkEvent(
            int lessonId, DateTime finishDateTime, string homeWork) =>
           (LessonId, FinishDateTime, Homework) = 
            (lessonId, finishDateTime, homeWork);

        #endregion Constructors
    }
}

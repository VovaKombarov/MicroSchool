using Common.EventBus;
using System;

namespace ParentApi.IntegrationEvents.Events
{
    /// <summary>
    /// Событие создания домашней работы.
    /// </summary>
    public class CreateHomeworkEvent : IntegrationEvent
    {
        #region Properties

        /// <summary>
        /// Наименование.
        /// </summary>
        public string Name => typeof(CreateHomeworkEvent).Name;

        /// <summary>
        /// Идентификатор урока.
        /// </summary>
        public int LessonId { get; }

        /// <summary>
        /// Время окончания домашней работы.
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
            int lessonId, DateTime finishDateTime, string homeWork)
        {
            LessonId = lessonId;
            FinishDateTime = finishDateTime;
            Homework = homeWork;
        }

        #endregion Constructors
    }
}

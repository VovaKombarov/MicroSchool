using Common.EventBus;
using System;

namespace ParentApi.IntegrationEvents.Events
{
    /// <summary>
    /// Событие создания встречи родителя и учителя.
    /// </summary>
    public class CreateParentTeacherMeetingEvent : IntegrationEvent
    {
        #region Properties

        /// <summary>
        /// Наименование события.
        /// </summary>
        public string Name => typeof(CreateParentTeacherMeetingEvent).Name;

        /// <summary>
        /// Идентификатор родителя.
        /// </summary>
        public int ParentId { get; }

        /// <summary>
        /// Идентификатор учителя.
        /// </summary>
        public int TeacherId { get;}

        /// <summary>
        /// Идентификатор студента.
        /// </summary>
        public int StudentId { get; }

        /// <summary>
        /// Время митинга.
        /// </summary>
        public DateTime MeetingDT { get; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parentId">Идентификатор родителя.</param>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="meetingDT">Время митинга.</param>
        public CreateParentTeacherMeetingEvent(
            int parentId,
            int teacherId,
            int studentId,
            DateTime meetingDT)
        {
            ParentId = parentId;
            TeacherId = teacherId;
            StudentId = studentId;
            MeetingDT = meetingDT;
        }

        #endregion Constructors
    }
}

using System;
using Common.EventBus;

namespace ParentApi.IntegrationEvents
{
    /// <summary>
    /// Событие создания встречи учителя и родителя.
    /// </summary>
    public class CreateTeacherParentMeetingEvent : IntegrationEvent
    {
        #region Properties

        /// <summary>
        /// Наименование события.
        /// </summary>
        public string Name => typeof(CreateTeacherParentMeetingEvent).Name;

        /// <summary>
        /// Идентификатор родителя.
        /// </summary>
        public int ParentId { get; }

        /// <summary>
        /// Идентификатор учителя.
        /// </summary>
        public int TeacherId { get; }

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
        public CreateTeacherParentMeetingEvent(
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

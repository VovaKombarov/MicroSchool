using Common.EventBus;
using System;

namespace TeacherApi.IntegrationEvents.Events
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
        /// Идентификатор учителя.
        /// </summary>
        public int TeacherId { get; }

        /// <summary>
        /// Идентификатор родителя.
        /// </summary>
        public int ParentId { get; }

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
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <param name="parentId">Идентификатор родителя.</param>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="meetingDT">Время митинга.</param>
        public CreateParentTeacherMeetingEvent(
            int teacherId, int parentId, int studentId, DateTime meetingDT)
        {
            TeacherId = teacherId;
            ParentId = parentId;
            StudentId = studentId;
            MeetingDT = meetingDT;
        }

        #endregion Constructors

    }
}

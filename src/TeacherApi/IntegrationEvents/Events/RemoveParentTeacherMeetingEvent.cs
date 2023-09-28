using Common.EventBus;

namespace TeacherApi.IntegrationEvents.Events
{
    /// <summary>
    /// Событие удаления встречи родителя и учителя.
    /// </summary>
    public class RemoveParentTeacherMeetingEvent : IntegrationEvent
    {
        #region Properties 

        /// <summary>
        /// Наименование события.
        /// </summary>
        public string Name => typeof(RemoveParentTeacherMeetingEvent).Name;

        /// <summary>
        /// Идентификатор встречи учителя и родителя.
        /// </summary>
        public int TeacherParentMeetingId { get; }

        #endregion Properties

        #region Constructors 

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="teacherParentMeetingId">Идентификатор встречи родителя и учителя.</param>
        public RemoveParentTeacherMeetingEvent(int teacherParentMeetingId)
        {
            TeacherParentMeetingId = teacherParentMeetingId;
        }

        #endregion Constructors
    }
}

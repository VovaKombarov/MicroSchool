using Common.EventBus;

namespace TeacherApi.IntegrationEvents.Events
{
    public class RemoveParentTeacherMeetingEvent : IntegrationEvent
    {
        public string Name => typeof(RemoveParentTeacherMeetingEvent).Name;

        public int TeacherParentMeetingId { get; }

        public RemoveParentTeacherMeetingEvent(int teacherParentMeetingId)
        {
            TeacherParentMeetingId = teacherParentMeetingId;
        }
    }
}

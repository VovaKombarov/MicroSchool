using Common.EventBus;

namespace ParentApi.IntegrationEvents.Events
{
    public class RemoveTeacherParentMeetingEvent : IntegrationEvent
    {
        public string Name => typeof(RemoveTeacherParentMeetingEvent).Name;

        public int TeacherParentMeetingId { get;}

        public RemoveTeacherParentMeetingEvent(int teacherParentMeetingId)
        {
            TeacherParentMeetingId = teacherParentMeetingId;
        }
    }
}

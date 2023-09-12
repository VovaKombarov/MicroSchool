using Common.EventBus;
using System;

namespace TeacherApi.IntegrationEvents.Events
{
    public class CreateParentTeacherMeetingEvent : IntegrationEvent
    {
        public int TeacherId { get; }

        public int ParentId { get; }

        public int StudentId { get; }

        public DateTime MeetingDT { get; }

        public string Name => typeof(CreateParentTeacherMeetingEvent).Name;

        public CreateParentTeacherMeetingEvent(int teacherId, int parentId, int studentId, DateTime meetingDT)
        {
            TeacherId = teacherId;
            ParentId = parentId;
            StudentId = studentId;
            MeetingDT = meetingDT;
        }
    }
}

using Common.EventBus;
using System;
using System.Reflection;

namespace TeacherApi.IntegrationEvents
{
    public class CreateTeacherParentMeetingEvent : IntegrationEvent
    {
        public int TeacherId { get; }

        public int ParentId { get; }

        public int StudentId { get; }

        public DateTime MeetingDT { get; }

        public string Name => typeof(CreateTeacherParentMeetingEvent).Name;

        public CreateTeacherParentMeetingEvent(int teacherId, int parentId, int studentId, DateTime meetingDT)
        {
            TeacherId = teacherId;
            ParentId = parentId;
            StudentId = studentId;
            MeetingDT = meetingDT;
        }
    }
}

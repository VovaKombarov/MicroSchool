using Common.EventBus;
using System;

namespace ParentApi.IntegrationEvents.Events
{
    public class CreateParentTeacherMeetingEvent : IntegrationEvent
    {
        public int ParentId { get; }

        public int TeacherId { get;}

        public int StudentId { get; }

        public DateTime MeetingDT { get; }

        public string Name => typeof(CreateParentTeacherMeetingEvent).Name;  
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
    }
}

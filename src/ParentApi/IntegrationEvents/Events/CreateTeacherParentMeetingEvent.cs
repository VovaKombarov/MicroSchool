using System;
using Common.EventBus;

namespace ParentApi.IntegrationEvents
{

    // Integration Events notes: 
    // An Event is “something that has happened in the past”, therefore its name has to be past tense
    // An Integration Event is an event that can cause side effects to other microservices, Bounded-Contexts or external systems.
    /// <summary>
    /// Событие интеграции по сути представляет собой класс для хранения данных.
    /// </summary>
    public class CreateTeacherParentMeetingEvent : IntegrationEvent
    {
        public int ParentId { get; }

        public int TeacherId { get; }

        public int StudentId { get; }

        public DateTime MeetingDT { get; }

        public string Name => typeof(CreateTeacherParentMeetingEvent).Name;

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
    }
}

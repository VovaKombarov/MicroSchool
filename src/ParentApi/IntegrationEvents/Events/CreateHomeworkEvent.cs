using Common.EventBus;
using System;

namespace ParentApi.IntegrationEvents.Events
{
    public class CreateHomeworkEvent : IntegrationEvent
    {
        public string Name => typeof(CreateHomeworkEvent).Name;

        public int LessonId { get; }

        public DateTime FinishDateTime { get; }

        public string Homework { get; }

        public CreateHomeworkEvent(
            int lessonId, DateTime finishDateTime, string homeWork)
        {
            LessonId = lessonId;
            FinishDateTime = finishDateTime;
            Homework = homeWork;
        }     
    }
}

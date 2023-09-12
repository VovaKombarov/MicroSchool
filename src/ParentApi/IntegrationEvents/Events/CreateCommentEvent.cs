using Common.EventBus;

namespace ParentApi.IntegrationEvents.Events
{
    public class CreateCommentEvent : IntegrationEvent
    {
        public string Name => typeof(CreateCommentEvent).Name;

        public int StudentId { get;}

        public int LessonId { get; }

        public string Comment { get; }

        public CreateCommentEvent(int studentId,  int lessonId, string comment)
        {
            StudentId = studentId; 
            LessonId = lessonId;
            Comment = comment;
        }
    }
}

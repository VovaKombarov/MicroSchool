using Common.EventBus;

namespace TeacherApi.IntegrationEvents.Events
{
    public class ChangeStatusHomeworkEvent : IntegrationEvent
    {
        public string Name => typeof(ChangeStatusHomeworkEvent).Name;

        public int StudentId { get;}

        public int LessonId { get; }

        public int HomeworkStatusId { get;}

        public ChangeStatusHomeworkEvent(
            int studentId, int lessonId, int homeworkStatusId)
        {
            StudentId = studentId;
            LessonId = lessonId;
            HomeworkStatusId = homeworkStatusId;
        }
    }
}

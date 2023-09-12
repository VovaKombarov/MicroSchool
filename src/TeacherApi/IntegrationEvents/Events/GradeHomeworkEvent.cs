using Common.EventBus;

namespace TeacherApi.IntegrationEvents.Events
{
    public class GradeHomeworkEvent : IntegrationEvent
    {
        public string Name => typeof(GradeHomeworkEvent).Name;

        public int StudentId { get; }

        public int LessonId { get; }

        public int Grade { get; }

        public GradeHomeworkEvent(int studentId, int lessonId, int grade)
        {
            StudentId = studentId;
            LessonId = lessonId;
            Grade = grade;
        }
    }
}

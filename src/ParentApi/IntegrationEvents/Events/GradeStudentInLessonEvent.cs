using Common.EventBus;
using System.Reflection;

namespace ParentApi.IntegrationEvents.Events
{
    public class GradeStudentInLessonEvent : IntegrationEvent
    {
        public string Name => typeof(GradeStudentInLessonEvent).Name;

        public int StudentId { get; }

        public int LessonId { get; }

        public int Grade { get; }

        public GradeStudentInLessonEvent(int studentId, int lessonId, int grade)
        {
            StudentId = studentId;
            LessonId = lessonId;
            Grade = grade;
        }
    }
}

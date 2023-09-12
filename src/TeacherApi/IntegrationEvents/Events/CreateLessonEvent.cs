using Common.EventBus;
using System;

namespace TeacherApi.IntegrationEvents.Events
{
    public class CreateLessonEvent : IntegrationEvent
    {
        public string Name => typeof(CreateLessonEvent).Name;

        public int TeacherId { get; }

        public int ClassId { get; }

        public int SubjectId { get; }

        public string Theme { get; }

        public DateTime LessonDateTime { get; }

        public CreateLessonEvent(
            int teacherId, 
            int classId, 
            int subjectId, 
            string theme, 
            DateTime lessonDateTime ) {

            TeacherId = teacherId;
            ClassId = classId;
            SubjectId = subjectId;
            Theme = theme;  
            LessonDateTime = lessonDateTime;
        }

    }
}

using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    public class StudentInLessonSpecificationFilterByLesson : BaseSpecification<StudentInLesson>
    {
        public StudentInLessonSpecificationFilterByLesson(int lessonId) : base(w => w.Lesson.Id == lessonId)
        {

        }
    }
}

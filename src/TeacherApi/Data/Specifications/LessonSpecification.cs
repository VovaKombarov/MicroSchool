using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    public class LessonSpecification : BaseSpecification<Lesson>
    {
        public LessonSpecification(int lessonId) : base(w => w.Id == lessonId) 
        {

        }
    }
}

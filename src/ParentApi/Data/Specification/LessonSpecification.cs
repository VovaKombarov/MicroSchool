using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    public class LessonSpecification : BaseSpecification<Lesson>
    {
        public LessonSpecification(int lessonId) : base(w => w.Id == lessonId)
        { 

        }
    }
}

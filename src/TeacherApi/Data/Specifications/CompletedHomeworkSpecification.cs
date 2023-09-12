using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    public class CompletedHomeworkSpecification : BaseSpecification<CompletedHomework>
    {
        public CompletedHomeworkSpecification(int studentInLessonId) 
            : base(w => w.StudentInLesson.Id == studentInLessonId)  
        { 

        }
    }
}

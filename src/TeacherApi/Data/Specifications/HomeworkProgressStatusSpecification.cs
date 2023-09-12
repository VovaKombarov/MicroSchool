using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    public class HomeworkProgressStatusSpecification : BaseSpecification<HomeworkProgressStatus>
    {
        public HomeworkProgressStatusSpecification(int studentInLessonId) : 
            base(x => x.StudentInLesson.Id == studentInLessonId)
        {

        }
    }
}

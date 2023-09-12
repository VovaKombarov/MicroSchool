using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    public class CompletedHomeworkSpecification : BaseSpecification<CompletedHomework>
    {
        public CompletedHomeworkSpecification(int studentInLesson)
        {
            Criteria = x => x.StudentInLesson.Id == studentInLesson;
        }
    }
}

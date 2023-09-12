using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    public class HomeworkStatusSpecification : BaseSpecification<HomeworkStatus>
    {
        public HomeworkStatusSpecification(int homeworkStatusId) : base(w => w.Id == homeworkStatusId) 
        {

        }
    }
}

using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    public class SubjectSpecification : BaseSpecification<Subject>
    {
        public SubjectSpecification(int subjectId) : base(w => w.Id == subjectId)
        { 

        }
    }
}

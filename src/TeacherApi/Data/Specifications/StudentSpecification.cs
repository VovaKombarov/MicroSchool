using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    public class StudentSpecification : BaseSpecification<Student>
    {
        public StudentSpecification(int studentId) : base(x => x.Id == studentId)
        {

        }
    }
}

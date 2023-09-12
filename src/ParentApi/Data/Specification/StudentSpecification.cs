using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    public class StudentSpecification : BaseSpecification<Student>
    {
        public StudentSpecification(int studentId) : base(w => w.Id == studentId)
        { 

        }
    }
}

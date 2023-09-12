using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    public class TeacherSpecification : BaseSpecification<Teacher>
    {
        public TeacherSpecification(int teacherId) : base(w => w.Id == teacherId) 
        {

        }
    }
}

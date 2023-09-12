using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    public class StudentsSpecification : BaseSpecification<Student>
    {
        public StudentsSpecification(int classId) : base(x => x.Class.Id == classId)
        {
            AddInclude(x => x.Class);
            AddOrderByDescending(w => w.Surname);
        } 
    }
}

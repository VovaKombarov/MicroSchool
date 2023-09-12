using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    public class ClassSpecification : BaseSpecification<Class>
    {
        public ClassSpecification(int classId) : base(x => x.Id == classId)
        {

        }
    }
}

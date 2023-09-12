using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    public class ParentSpecification : BaseSpecification<Parent>
    {
        public ParentSpecification(int parentId) : base(w => w.Id == parentId) 
        {

        }
    }
}

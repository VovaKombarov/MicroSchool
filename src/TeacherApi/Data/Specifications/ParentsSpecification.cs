using System.Linq;
using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    public class ParentsSpecification : BaseSpecification<Parent>
    {
        public ParentsSpecification(int studentId) : base(x => x.Students.Any(a => a.Id == studentId)) 
        {

        }
    }
}

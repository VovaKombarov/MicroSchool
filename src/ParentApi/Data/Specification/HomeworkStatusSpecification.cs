using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    public class HomeworkStatusSpecification : BaseSpecification<HomeworkStatus>
    {
        public HomeworkStatusSpecification(int id) : base(w => w.Id ==  id) 
        {

        } 
    }
}

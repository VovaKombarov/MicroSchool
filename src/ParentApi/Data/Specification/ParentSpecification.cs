using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    public class ParentSpecification : BaseSpecification<Parent>
    {
        public ParentSpecification(int parentId) 
            : base(x => x.Id == parentId) { }
    }
}

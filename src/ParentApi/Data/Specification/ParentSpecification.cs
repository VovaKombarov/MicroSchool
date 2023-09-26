using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    /// <summary>
    /// Спецификация родителя.
    /// </summary>
    public class ParentSpecification : BaseSpecification<Parent>
    {
        /// <summary>
        /// Получает родителя по идентификатору родителя.
        /// </summary>
        /// <param name="parentId">Идентификатор родителя.</param>
        public ParentSpecification(int parentId) 
            : base(x => x.Id == parentId) { }
    }
}

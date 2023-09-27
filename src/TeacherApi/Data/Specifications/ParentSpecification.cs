using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    /// <summary>
    /// Спецификация родителя.
    /// </summary>
    public class ParentSpecification : BaseSpecification<Parent>
    {
        #region Constructors 

        /// <summary>
        /// Получает родителя по идентификатору родителя.
        /// </summary>
        /// <param name="parentId"></param>
        public ParentSpecification(int parentId) : base(w => w.Id == parentId)
        {

        }

        #endregion Constructors
    }
}

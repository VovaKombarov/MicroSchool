using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    /// <summary>
    /// Спецификация для получения класса.
    /// </summary>
    public class ClassSpecification : BaseSpecification<Class>
    {
        #region Constructors 

        /// <summary>
        ///  Получает класс по идентификатору класса.
        /// </summary>
        /// <param name="classId">Идентификатор класса.</param>
        public ClassSpecification(int classId) : base(x => x.Id == classId)
        {

        }

        #endregion Constructors
    }
}

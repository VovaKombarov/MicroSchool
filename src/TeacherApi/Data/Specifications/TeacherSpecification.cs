using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    /// <summary>
    /// Спецификация учителя.
    /// </summary>
    public class TeacherSpecification : BaseSpecification<Teacher>
    {
        #region Constructors 

        /// <summary>
        /// Получает учителя по идентификатору.
        /// </summary>
        /// <param name="teacherId">Идентификатор учителя.</param>
        public TeacherSpecification(int teacherId) : 
            base(w => w.Id == teacherId)
        {

        }

        #endregion Constructors
    }
}

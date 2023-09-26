using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    /// <summary>
    /// Спецификация учителя.
    /// </summary>
    public class TeacherSpecification : BaseSpecification<Teacher>
    {
        /// <summary>
        /// Получает учителя по идентификатору учителя.
        /// </summary>
        /// <param name="teacherId">Идентификатор учителя.</param>
        public TeacherSpecification(int teacherId) : base(w => w.Id == teacherId) 
        {

        }
    }
}

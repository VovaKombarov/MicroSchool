using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    /// <summary>
    /// Спецификация студента.
    /// </summary>
    public class StudentSpecification : BaseSpecification<Student>
    {
        /// <summary>
        /// Получает студента по идентификатору студента.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        public StudentSpecification(int studentId) : base(w => w.Id == studentId)
        { 

        }
    }
}

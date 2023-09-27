using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    /// <summary>
    /// Спецификация студента.
    /// </summary>
    public class StudentSpecification : BaseSpecification<Student>
    {
        #region Constructors 

        /// <summary>
        /// Получает студента по идентификатору студента.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        public StudentSpecification(int studentId) : 
            base(x => x.Id == studentId)
        {

        }

        #endregion Constructors
    }
}

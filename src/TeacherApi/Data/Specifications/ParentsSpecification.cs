using System.Linq;
using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    /// <summary>
    /// Спецификация родителей.
    /// </summary>
    public class ParentsSpecification : BaseSpecification<Parent>
    {
        #region Constructors 

        /// <summary>
        /// Получает родителей по идентификатору студента.
        /// </summary>
        /// <param name="studentId">идентификатор студента</param>
        public ParentsSpecification(int studentId) :
           base(x => x.Students.Any(a => a.Id == studentId))
        {

        }

        #endregion Constructors

    }
}

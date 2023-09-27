using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    /// <summary>
    /// Спецификация предмета.
    /// </summary>
    public class SubjectSpecification : BaseSpecification<Subject>
    {
        #region Constructors 

        /// <summary>
        /// Получает предмет по идентификатору предмета.
        /// </summary>
        /// <param name="subjectId">Идентификатор предмета.</param>
        public SubjectSpecification(int subjectId) : 
            base(w => w.Id == subjectId)
        {

        }

        #endregion Constructors
    }
}

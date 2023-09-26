using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    /// <summary>
    /// Спецификация предмета.
    /// </summary>
    public class SubjectSpecification : BaseSpecification<Subject>
    {
        /// <summary>
        /// Получает предмета по идентификатору предмета.
        /// </summary>
        /// <param name="subjectId">Идентификатор предмета.</param>
        public SubjectSpecification(int subjectId) : base(w => w.Id == subjectId)
        {

        }
    }
}

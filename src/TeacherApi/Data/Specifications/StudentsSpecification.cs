using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    /// <summary>
    /// Спецификация студентов.
    /// </summary>
    public class StudentsSpecification : BaseSpecification<Student>
    {
        #region Constructors 

        /// <summary>
        /// Получает коллекцию студентов по идентификатору класса.
        /// Добавляет связанные данные класса, упорядочивает по убыванию.
        /// </summary>
        /// <param name="classId">Идентификатор класса.</param>
        public StudentsSpecification(int classId) : base(x => x.Class.Id == classId)
        {
            AddInclude(x => x.Class);
            AddOrderByDescending(w => w.Surname);
        }

        #endregion Constructors

    }
}

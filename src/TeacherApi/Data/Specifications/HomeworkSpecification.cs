using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    /// <summary>
    /// Спецификация домашней работы.
    /// </summary>
    public class HomeworkSpecification : BaseSpecification<Homework>
    {
        #region Constructors 

        /// <summary>
        /// Получает домашнюю работу по идентификатору домашней работы.
        /// </summary>
        /// <param name="homeworkId">Идентификатор домашней работы.</param>
        public HomeworkSpecification(int homeworkId) : 
            base(w => w.Id == homeworkId)
        {

        }

        #endregion Constructors
    }

    /// <summary>
    /// Спецификация домашней работы по уроку.
    /// </summary>
    public class HomeworkSpecificationByLesson : BaseSpecification<Homework>
    {
        #region Constructors 

        /// <summary>
        /// Получает домашнюю работу по идентификатору урока.
        /// </summary>
        /// <param name="lessonId">идентификатор урока</param>
        public HomeworkSpecificationByLesson(int lessonId) :
          base(w => w.Id == lessonId)
        {

        }

        #endregion Constructors
    }
}

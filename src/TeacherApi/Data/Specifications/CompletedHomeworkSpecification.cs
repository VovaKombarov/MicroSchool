using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    /// <summary>
    /// Спецификация для готовой домашней работы.
    /// </summary>
    public class CompletedHomeworkSpecification : BaseSpecification<CompletedHomework>
    {
        #region Constructors 

        /// <summary>
        /// Получает готовую домашнюю работу по идентификатору студента на уроке.
        /// </summary>
        /// <param name="studentInLessonId">Идентификатор студента на уроке.</param>
        public CompletedHomeworkSpecification(int studentInLessonId)
          : base(w => w.StudentInLesson.Id == studentInLessonId)
        {

        }

        #endregion Constructors
    }
}

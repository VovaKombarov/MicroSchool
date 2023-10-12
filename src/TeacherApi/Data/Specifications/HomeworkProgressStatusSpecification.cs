using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    /// <summary>
    /// Спецификация статуса прогресса домашней работы.
    /// </summary>
    public class HomeworkProgressStatusSpecification : 
        BaseSpecification<HomeworkProgressStatus>
    {
        #region Constructors 

        /// <summary>
        /// Получает статус прогресса домашней работы по идентификатору студента на уроке.
        /// </summary>
        /// <param name="studentInLessonId">Идентификатор студента на уроке.</param>
        public HomeworkProgressStatusSpecification(int studentInLessonId) :
           base(x => x.StudentInLesson.Id == studentInLessonId)
        {

        }

        #endregion Constructors
    }
}

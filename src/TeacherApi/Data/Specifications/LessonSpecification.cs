using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    /// <summary>
    /// Спецификация урока.
    /// </summary>
    public class LessonSpecification : BaseSpecification<Lesson>
    {
        #region Constructors 

        /// <summary>
        /// Получает урок по идентификатору урока.
        /// </summary>
        /// <param name="lessonId">идентификатор урока</param>
        public LessonSpecification(int lessonId) : base(w => w.Id == lessonId)
        {

        }

        #endregion Constructors

    }
}

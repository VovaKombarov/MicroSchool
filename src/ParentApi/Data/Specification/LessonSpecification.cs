using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    /// <summary>
    /// Спецификация для урока.
    /// </summary>
    public class LessonSpecification : BaseSpecification<Lesson>
    {
        /// <summary>
        /// Получает урок по идентификатору урока.
        /// </summary>
        /// <param name="lessonId"></param>
        public LessonSpecification(int lessonId) : base(w => w.Id == lessonId)
        { 

        }
    }
}

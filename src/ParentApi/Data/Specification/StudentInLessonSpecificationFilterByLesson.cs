using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    /// <summary>
    /// Спецификация студента на уроке фильтрующая по уроку.
    /// </summary>
    public class StudentInLessonSpecificationFilterByLesson : BaseSpecification<StudentInLesson>
    {
        /// <summary>
        /// Получает студента на уроке по идентификатору урока.
        /// </summary>
        /// <param name="lessonId">Идентификатор урока.</param>
        public StudentInLessonSpecificationFilterByLesson(int lessonId) : base(w => w.Lesson.Id == lessonId)
        {

        }
    }
}

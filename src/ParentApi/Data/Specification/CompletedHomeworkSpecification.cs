using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    /// <summary>
    /// Спецификация используемая для готовой домашней работы.
    /// </summary>
    public class CompletedHomeworkSpecification : BaseSpecification<CompletedHomework>
    {
        /// <summary>
        /// Получает готовую работу по идентификатору студента на уроке.
        /// </summary>
        /// <param name="studentInLesson">Идентификатор студента на уроке.</param>
        public CompletedHomeworkSpecification(int studentInLesson)
        {
            Criteria = x => x.StudentInLesson.Id == studentInLesson;
        }
    }
}

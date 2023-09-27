using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    /// <summary>
    /// Спецификация статуса домашней работы.
    /// </summary>
    public class HomeworkStatusSpecification : BaseSpecification<HomeworkStatus>
    {
        #region Constructors 

        /// <summary>
        /// Получает статус домашней работы по идентификатору статуса домашней работы.
        /// </summary>
        /// <param name="homeworkStatusId"></param>
        public HomeworkStatusSpecification(int homeworkStatusId) : 
            base(w => w.Id == homeworkStatusId)
        {

        }

        #endregion Constructors
    }
}

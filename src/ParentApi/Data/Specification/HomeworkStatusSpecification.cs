using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    /// <summary>
    /// Спецификация для статуса домашней работы.
    /// </summary>
    public class HomeworkStatusSpecification : BaseSpecification<HomeworkStatus>
    {
        /// <summary>
        /// Получает статус домашней работы по идентификатору.
        /// </summary>
        /// <param name="id"></param>
        public HomeworkStatusSpecification(int id) : base(w => w.Id ==  id) 
        {

        } 
    }
}

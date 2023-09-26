using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    /// <summary>
    /// Спецификация для обьекта учитель/класс/предмет.
    /// </summary>
    public class TeacherClassSubjectSpecification : BaseSpecification<TeacherClassSubject>
    {
        /// <summary>
        /// Получает обьект учитель/класс/предмет по идентификатору учителя, класса, предмета.
        /// Добавляет связанные данные учителя, класса, предмета.
        /// </summary>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <param name="classId">Идентификатор класса.</param>
        /// <param name="subjectId">Идентификатор предмета.</param>
        public TeacherClassSubjectSpecification(int teacherId, int classId, int subjectId) 
        {
            AddInclude(w => w.Teacher);
            AddInclude(w => w.Class);
            AddInclude(w => w.Subject);

            Criteria = w => w.Teacher.Id == teacherId && w.Class.Id == classId && w.Subject.Id == subjectId;
        }
    }
}

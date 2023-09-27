using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    /// <summary>
    /// Спецификация обьекта учитель/класс/предмет.
    /// </summary>
    public class TeacherClassSubjectSpecification : BaseSpecification<TeacherClassSubject>
    {
        #region Constructors 

        /// <summary>
        /// Получает обьект учитель/класс/предмет по идентификатору учителя, класса, предмета.
        /// </summary>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <param name="classId">Идентификатор класса.</param>
        /// <param name="subjectId">Идентификатор предмета.</param>
        public TeacherClassSubjectSpecification(
            int teacherId, int classId, int subjectId)
            : base(w => w.Teacher.Id == teacherId && w.Class.Id == classId && 
                        w.Subject.Id == subjectId)
        {
            AddInclude(w => w.Subject);
            AddInclude(w => w.Class);
            AddInclude(w => w.Teacher);
        }

        #endregion Constructors

    }
}

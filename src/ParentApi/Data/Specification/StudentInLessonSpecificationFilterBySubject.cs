using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    /// <summary>
    /// Спецификация студента на уроке фильтрующая по предмету.
    /// </summary>
    public class StudentInLessonSpecificationFilterBySubject : BaseSpecification<StudentInLesson>
    {
        /// <summary>
        /// Получает студента на уроке по идентификатору студента и идентификатору предмета.
        /// Добавляет связанные данные, студент, урок, учитель/класс/предмет, предмет.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="subjectId">Идентификатор предмета.</param>
        public StudentInLessonSpecificationFilterBySubject(int studentId, int subjectId)
        {
            AddInclude(w => w.Student);
            AddInclude(w => w.Lesson.TeacherClassSubject.Subject);

            Criteria = x => x.Student.Id == studentId &&
                    x.Lesson.TeacherClassSubject.Subject.Id == subjectId; 
        }
    }
}

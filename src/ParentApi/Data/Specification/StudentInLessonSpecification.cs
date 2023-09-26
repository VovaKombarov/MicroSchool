using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    /// <summary>
    /// Спецификация для студента на уроке.
    /// </summary>
    public class StudentInLessonSpecification : BaseSpecification<StudentInLesson>
    {
        /// <summary>
        /// Получает студента на уроке в зависимости от идентификатора студента и 
        /// идентификатора урока.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        public StudentInLessonSpecification(int studentId, int lessonId)
        {
            if (studentId != 0 && lessonId != 0)
                Criteria = x => x.Student.Id == studentId && x.Lesson.Id == lessonId;
            else if (studentId != 0 && lessonId == 0)
                Criteria = x => x.Student.Id == studentId;
            else if (studentId == 0 && lessonId != 0)
                Criteria = x => x.Lesson.Id == lessonId;
            else
                Criteria = x => false;
        }

        /// <summary>
        /// Получает студента на уроке по идентификатору студента. 
        /// Добавляет связанные данные студента и урок, обьект учитель/класс/предмет, предмет.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        public StudentInLessonSpecification(int studentId)
        {
            AddInclude(w => w.Student);
            AddInclude(w => w.Lesson.TeacherClassSubject.Subject);

            Criteria = x => x.Student.Id == studentId;
        }

    }
}

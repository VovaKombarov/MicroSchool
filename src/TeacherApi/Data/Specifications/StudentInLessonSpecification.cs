using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    /// <summary>
    /// Спецификация студентов на уроке.
    /// </summary>
    public class StudentInLessonSpecification : BaseSpecification<StudentInLesson>
    {
        #region Constructors 

        /// <summary>
        /// Получает студентов на уроке в зависимости от идентификатора студента и 
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
        /// Получает студентов на уроке по идентификатору урока 
        /// и добавляет связанные данные урока и студента.
        /// </summary>
        /// <param name="lessonId">Идентификатор урока.</param>
        public StudentInLessonSpecification(int lessonId)
        {
            Criteria = x => x.Lesson.Id == lessonId;
            AddInclude(w => w.Lesson);
            AddInclude(w => w.Student);
        }

        #endregion Constructors
    }
}

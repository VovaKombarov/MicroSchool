using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    public class StudentInLessonSpecification : BaseSpecification<StudentInLesson>
    {
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

        public StudentInLessonSpecification(int studentId)
        {
            AddInclude(w => w.Student);
            AddInclude(w => w.Lesson.TeacherClassSubject.Subject);
            Criteria = x => x.Student.Id == studentId;
        }

    }
}

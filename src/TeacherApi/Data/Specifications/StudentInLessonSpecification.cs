using System.Linq.Expressions;
using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
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

        public StudentInLessonSpecification(int lessonId)
        {
            Criteria = x => x.Lesson.Id == lessonId;
            AddInclude(w => w.Lesson);
            AddInclude(w => w.Student);
        }
    }
}

using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    public class StudentInLessonSpecificationFilterBySubject : BaseSpecification<StudentInLesson>
    {
        public StudentInLessonSpecificationFilterBySubject(int studentId, int subjectId)
        {
            AddInclude(w => w.Student);
            AddInclude(w => w.Lesson.TeacherClassSubject.Subject);

            Criteria = x => x.Student.Id == studentId &&
                    x.Lesson.TeacherClassSubject.Subject.Id == subjectId; 
        }
    }
}

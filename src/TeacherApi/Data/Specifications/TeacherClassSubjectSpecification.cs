using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    public class TeacherClassSubjectSpecification : BaseSpecification<TeacherClassSubject>
    {
        public TeacherClassSubjectSpecification(int teacherId, int classId, int subjectId) 
            : base(w => w.Teacher.Id == teacherId && w.Class.Id == classId && w.Subject.Id == subjectId) 
        {
            AddInclude(w => w.Subject);
            AddInclude(w => w.Class);
            AddInclude(w => w.Teacher);
        }

        
    }
}

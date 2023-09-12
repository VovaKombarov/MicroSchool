using ParentApi.Models;

namespace ParentApi.Data.Specifications
{
    public class TeacherClassSubjectSpecification : BaseSpecification<TeacherClassSubject>
    {
        public TeacherClassSubjectSpecification(int teacherId, int classId, int subjectId) 
        {
            AddInclude(w => w.Teacher);
            AddInclude(w => w.Class);
            AddInclude(w => w.Subject);
            Criteria = w => w.Teacher.Id == teacherId && w.Class.Id == classId && w.Subject.Id == subjectId;
        }
    }
}

using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    public class TeacherParentMeetingSpecification : BaseSpecification<TeacherParentMeeting>
    {
        public TeacherParentMeetingSpecification(int teacherParentMeetingId) 
            : base(w => w.Id == teacherParentMeetingId)  
        {

        }  
    }
}

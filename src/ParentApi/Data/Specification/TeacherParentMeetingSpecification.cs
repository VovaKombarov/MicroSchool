using ParentApi.Models;
using System;

namespace ParentApi.Data.Specifications
{
    public class TeacherParentMeetingSpecification : BaseSpecification<TeacherParentMeeting>
    {
        public TeacherParentMeetingSpecification(
            int studentId, int teacherId, int parentId, DateTime meetingDT) : base(x =>
                x.Student.Id == studentId &&
                x.Teacher.Id == teacherId &&
                x.Parent.Id == parentId &&
                x.MeetingDT == meetingDT)
        {

        }

        public TeacherParentMeetingSpecification(int teacherParentMeetingSpecificationId) 
            : base(w => w.Id == teacherParentMeetingSpecificationId) 
        { 

        }
    }

}

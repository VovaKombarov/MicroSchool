using ParentApi.Models;
using System;

namespace ParentApi.Data.Specifications
{
    /// <summary>
    /// Спецификация для обьекта встречи учителя и родителя.
    /// </summary>
    public class TeacherParentMeetingSpecification : BaseSpecification<TeacherParentMeeting>
    {
        /// <summary>
        /// Получает обьект встречи учителя и родителя по идентификатору студента, 
        /// учителя, родителя, времени митинга.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <param name="parentId">Идентификатор родителя.</param>
        /// <param name="meetingDT">Время митинга.</param>
        public TeacherParentMeetingSpecification(
            int studentId, int teacherId, int parentId, DateTime meetingDT) : base(x =>
                x.Student.Id == studentId &&
                x.Teacher.Id == teacherId &&
                x.Parent.Id == parentId &&
                x.MeetingDT == meetingDT)
        {

        }

        /// <summary>
        /// Получает обьект встречи учителя и родителя по идентификатору.
        /// </summary>
        /// <param name="teacherParentMeetingSpecificationId">Идентификатор обьекта встречи родителя и учителя.</param>
        public TeacherParentMeetingSpecification(int teacherParentMeetingSpecificationId) 
            : base(w => w.Id == teacherParentMeetingSpecificationId) 
        { 

        }
    }

}

using TeacherApi.Models;

namespace TeacherApi.Data.Specifications
{
    /// <summary>
    /// Спецификация встречи учителя и родителя.
    /// </summary>
    public class TeacherParentMeetingSpecification : 
        BaseSpecification<TeacherParentMeeting>
    {
        #region Constructors 

        /// <summary>
        /// Получает встречу учителя и родителя по идентификатору.
        /// </summary>
        /// <param name="teacherParentMeetingId">Идентификатор встречи учителя и родителя.</param>
        public TeacherParentMeetingSpecification(int teacherParentMeetingId)
           : base(w => w.Id == teacherParentMeetingId)
        {

        }

        #endregion Constructors

    }
}

using Common.EventBus;
using ParentApi.IntegrationEvents.EventHandling;
using ParentApi.IntegrationEvents.Events;
using System.Threading.Tasks;

namespace ParentApi.IntegrationEvents.Services
{
    public interface IParentIntegrationEventService 
    {
        Task CreateLessonAsync(CreateLessonEvent @event);

        Task CreateTeacherParentMeetingAsync(CreateTeacherParentMeetingEvent @event);

        Task CreateHomeworkAsync(CreateHomeworkEvent @event);

        Task ChangeStatusHomeworkAsync(ChangeStatusHomeworkEvent @event);

        Task GradeHomeworkAsync(GradeHomeworkEvent @event);

        Task CreateCommentAsync(CreateCommentEvent @event);

        Task GradeStudentInLessonAsync(GradeStudentInLessonEvent @event);

        Task RemoveTeacherParentMeetingAsync(RemoveTeacherParentMeetingEvent @event);
    }
}

